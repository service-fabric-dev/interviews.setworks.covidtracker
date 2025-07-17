using CovidTracker.Application.Queries;
using CovidTracker.Domain.Repositories;
using CovidTracker.Shared.DTOs;

using MassTransit;

namespace CovidTracker.Application.Handlers;

/// <summary>
/// CQRS query handler that retrieves the latest COVID-19 statistics.
/// </summary>
/// <param name="snapshotRepository">Repository for COVID stat snapshots</param>
public class GetLatestStatsQueryHandler(
        IStatSnapshotRepository snapshotRepository
    ) : IConsumer<GetLatestStatsQuery>
{
    private readonly IStatSnapshotRepository _snapshotRepository = snapshotRepository ?? throw new ArgumentNullException(nameof(snapshotRepository));

    public async Task Consume(ConsumeContext<GetLatestStatsQuery> context)
    {
        var results = await _snapshotRepository.GetLatestSnapshotAsync();

        await context.RespondAsync(new LatestStatsResponse([.. results.Stats]));
    }
}