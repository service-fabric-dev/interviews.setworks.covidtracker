using CovidTracker.Application.Commands;
using CovidTracker.Application.Services;
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
public class IngestStatsCommandHandler(
        IStatIngestionService ingestionService,
        IStatSnapshotRepository snapshotRepository,
        IIntegrationPublisherService integrationPublisher
    ) : IConsumer<IngestStatsCommand>
{
    private readonly IStatIngestionService _ingestionService = ingestionService ?? throw new ArgumentNullException(nameof(ingestionService));
    private readonly IStatSnapshotRepository _snapshotRepository = snapshotRepository ?? throw new ArgumentNullException(nameof(snapshotRepository));
    private readonly IIntegrationPublisherService _integrationPublisher = integrationPublisher ?? throw new ArgumentNullException(nameof(integrationPublisher));

    public async Task Consume(ConsumeContext<IngestStatsCommand> context)
    {
        var cancellation = context.CancellationToken;
        var stats = await _ingestionService.FetchCurrentStatsAsync(cancellation);
        
        await _snapshotRepository.SaveSnapshotAsync(new StateStatSnapshot(stats));
        await _integrationPublisher.PublishEvent(new StatsIngestedEvent(stats), context.CancellationToken); // Consumed by the WorkerService to trigger alert generation
    }
}
