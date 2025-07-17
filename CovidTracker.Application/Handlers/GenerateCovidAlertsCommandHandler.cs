using CovidTracker.Application.Commands;
using CovidTracker.Application.Services;
using CovidTracker.Domain.Events;
using CovidTracker.Domain.Models;
using CovidTracker.Domain.Repositories;

using MassTransit;

namespace CovidTracker.Application.Handlers;

/// <summary>
/// CQRS command handler that generates COVID-19 alerts based on the latest state statistics.
/// </summary>
/// <param name="snapshotRepository">Repository for COVID stat snapshots</param>
/// <param name="publisher">Integration event publisher</param>
public class GenerateCovidAlertsCommandHandler(
        IStatSnapshotRepository snapshotRepository,
        IIntegrationPublisherService publisher
    ) : IConsumer<GenerateCovidAlertsCommand>
{
    private readonly IStatSnapshotRepository _snapshotRepository = snapshotRepository ?? throw new ArgumentNullException(nameof(snapshotRepository));
    private readonly IIntegrationPublisherService _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));

    public async Task Consume(ConsumeContext<GenerateCovidAlertsCommand> context)
    {
        var cancellation = context.CancellationToken;
        var currentSnapshot = new StateStatSnapshot(context.Message.Stats);
        var previousSnapshot = await _snapshotRepository.GetLatestSnapshotAsync(cancellation);

        var alerts = currentSnapshot.GenerateAlerts(previousSnapshot).ToList();
        if (alerts.Count > 0)
        {
            await Parallel.ForEachAsync(alerts, cancellation, async (alert, ct) =>
            {
                var alertEvent = new AlertGeneratedEvent(alert);
                await _publisher.PublishEvent(alertEvent, ct); // This integration event will drive SignalR notifications in the Blazor UI
            });
        }

        await _snapshotRepository.SaveSnapshotAsync(currentSnapshot, cancellation);
    }
}
