using MassTransit;
using CovidTracker.Application.Commands;
using CovidTracker.Domain.Models;
using CovidTracker.Infrastructure.Messaging;
using CovidTracker.Infrastructure.Repositories;

namespace CovidTracker.Application.Handlers;

public class GenerateCovidAlertsCommandHandler(
        IStatSnapshotRepository snapshotRepository,
        IMessagePublisher messagePublisher
    ) : IConsumer<GenerateCovidAlertsCommand>
{
    private readonly IStatSnapshotRepository _snapshotRepository = snapshotRepository ?? throw new ArgumentNullException(nameof(snapshotRepository));
    private readonly IMessagePublisher _messagePublisher = messagePublisher ?? throw new ArgumentNullException(nameof(messagePublisher));

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
                await _messagePublisher.PublishCovidAlert(alert, ct);
            });
        }

        await _snapshotRepository.SaveSnapshotAsync(currentSnapshot, cancellation);
    }
}
