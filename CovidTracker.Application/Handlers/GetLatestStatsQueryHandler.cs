using CovidTracker.Application.Queries;
using CovidTracker.Shared.DTOs;
using CovidTracker.Infrastructure.Repositories;
using MassTransit;

namespace CovidTracker.Application.Handlers;

public class GetLatestStatsQueryHandler(
        IStatRepository statRepository
    ) : IConsumer<GetLatestStatsQuery>
{
    private readonly IStatRepository _statRepository = statRepository ?? throw new ArgumentNullException(nameof(statRepository));

    public async Task Consume(ConsumeContext<GetLatestStatsQuery> context)
    {
        var results = await _statRepository.GetLatestStatsAsync();

        await context.RespondAsync(new LatestStatsResponse(results));
    }
}