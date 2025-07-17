using CovidTracker.Shared.DTOs;
using CovidTracker.Web.Models;

namespace CovidTracker.Web.Services;

public class ApiService(HttpClient client)
{
    private readonly HttpClient _client = client ?? throw new ArgumentNullException(nameof(client));

    public async Task<LatestStatsResponse> GetStateStatsAsync()
    {
        var result = await _client.GetFromJsonAsync<LatestStatsResponse>("/api/stats/latest");
        return result ?? new([]);
    }
}
