using CovidTracker.Domain.Mapping;
using CovidTracker.Domain.Models;
using CovidTracker.Infrastructure.Http.Models;
using System.Net.Http.Json;

namespace CovidTracker.Infrastructure.Http;

public class CovidApiClient(HttpClient client)
{
    private readonly HttpClient _client = client ?? throw new ArgumentNullException(nameof(client));

    public async Task<List<StateStat>> FetchStateStatsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _client.GetAsync("states", cancellationToken);
        response.EnsureSuccessStatusCode();
        if (response.Content.Headers.ContentType?.MediaType != "application/json")
        {
            throw new InvalidOperationException("Unexpected content type from API.");
        }

        var dtos = await response.Content.ReadFromJsonAsync<List<CovidStateApiDto>>(cancellationToken);
        if (dtos == null)
        {
            throw new InvalidOperationException("Failed to deserialize state stats.");
        }

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
