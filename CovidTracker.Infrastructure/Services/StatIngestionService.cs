using CovidTracker.Domain.Mapping;
using CovidTracker.Domain.Models;
using CovidTracker.Domain.Services;
using CovidTracker.Infrastructure.Http;
using MassTransit.Initializers;

namespace CovidTracker.Infrastructure.Services;

public class StatIngestionService(CovidApiClient client) : IStatIngestionService
{
    private readonly CovidApiClient _client = client ?? throw new ArgumentNullException(nameof(client));

    public async Task<List<StateStat>> FetchCurrentStatsAsync(CancellationToken cancellation = default)
    {
        var dtos = await _client.FetchStateStatsAsync(cancellation);

        var stats = dtos.Select(dto => new StateStat
        {
            State = dto.State,
            StateCode = StateCodeMapper.TryGetCode(dto.State, out var stateCode) ? stateCode : "No known state code",
            TotalCases = dto.Cases,
            TodayCases = dto.TodayCases,
            Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(dto.Updated).UtcDateTime
        }).ToList();

        return stats;
    }
}
