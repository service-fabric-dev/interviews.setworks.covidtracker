
using CovidTracker.Application.Services;
using CovidTracker.Domain.Events;
using CovidTracker.Domain.Repositories;

using MassTransit;

namespace CovidTracker.WorkerService.Consumers;

/// <summary>
/// Consumer for the StatsIngestedEvent domain event that compares the two latest snapshots and generates alerts if necessary.
/// </summary>
public class StatsIngestedConsumer(
        IStatSnapshotRepository snapshotRepository,
        IIntegrationPublisherService integrationPublisher
    ) : IConsumer<StatsIngestedEvent>
{
    private readonly IStatSnapshotRepository _snapshotRepository = snapshotRepository ?? throw new ArgumentNullException(nameof(snapshotRepository));
    private readonly IIntegrationPublisherService _integrationPublisher = integrationPublisher ?? throw new ArgumentNullException(nameof(integrationPublisher));

    public async Task Consume(ConsumeContext<StatsIngestedEvent> context)
    {
        var cancellation = context.CancellationToken;
        var (previous, current) = await _snapshotRepository.GetLastTwoSnapshotsAsync(cancellation);

        var alerts = current.GenerateAlerts(previous).ToList();
        if (alerts.Count > 0)
        {
            await Parallel.ForEachAsync(alerts, cancellation, async (alert, ct) =>
            {
                var alertEvent = new AlertGeneratedEvent(alert);
                await _integrationPublisher.PublishEvent(alertEvent, ct); // This integration event will drive SignalR notifications in the Blazor UI
            });
        }
    }
}
