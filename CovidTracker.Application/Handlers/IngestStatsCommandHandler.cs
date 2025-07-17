using CovidTracker.Application.Commands;
using CovidTracker.Domain.Events;
using CovidTracker.Domain.Models;
using CovidTracker.Domain.Repositories;
using CovidTracker.Domain.Services;

using MassTransit;

namespace CovidTracker.Application.Handlers;

/// <summary>
/// CQRS command handler that fetches and saves the latest COVID-19 statistics.
/// </summary>
/// <param name="ingestionService">Service for fetching external COVID stats</param>
/// <param name="snapshotRepository">Repository for saving state snapshots</param>
/// <param name="publisher">Domain event publisher service</param>
public class IngestStatsCommandHandler(
        IStatIngestionService ingestionService,
        IStatSnapshotRepository snapshotRepository,
        IDomainPublisherService publisher
    ) : IConsumer<IngestStatsCommand>
{
    private readonly IStatIngestionService _ingestionService = ingestionService ?? throw new ArgumentNullException(nameof(ingestionService));
    private readonly IStatSnapshotRepository _snapshotRepository = snapshotRepository ?? throw new ArgumentNullException(nameof(snapshotRepository));
    private readonly IDomainPublisherService _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));

    public async Task Consume(ConsumeContext<IngestStatsCommand> context)
    {
        var stats = await _ingestionService.FetchCurrentStatsAsync(context.CancellationToken);
        await _snapshotRepository.SaveSnapshotAsync(new StateStatSnapshot(stats));
        await _publisher.PublishEvent(new StatsIngestedEvent(stats), context.CancellationToken); // Currently unused event, but can be used for further processing
    }
}
