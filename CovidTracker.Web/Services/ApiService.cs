using CovidTracker.Domain.Models;
using CovidTracker.Shared.DTOs;
using CovidTracker.Web.Models;
using static System.Net.WebRequestMethods;

namespace CovidTracker.Web.Services;

public class ApiService(HttpClient client)
{
    private readonly HttpClient _client = client ?? throw new ArgumentNullException(nameof(client));

    public Task<List<StateData>> GetStateDataAsync()
    {
        return Task.FromResult(new List<StateData>
        {
            new("California", "California", "Sunny coast, 39M people"),
            new("Texas", "Texas", "BBQ, 29M people"),
            new("New York", "New York", "Financial hub, 19M people"),
            // Use state names from GeoJSON’s `"properties.name"` field
        });
    }

    //public Task<List<StateStat>> GetStateStatsAsync()
    //{
    //    return Task.FromResult<List<StateStat>>([
    //        new() {
    //            State = "California",
    //            StateCode = "CA",
    //            TodayCases = 100,
    //            TotalCases = 12345,
    //            Timestamp = DateTime.UtcNow
    //        },
    //        new() {
    //            State = "Texas",
    //            StateCode = "TX",
    //            TodayCases = 200,
    //            TotalCases = 67890,
    //            Timestamp = DateTime.UtcNow
    //        },
    //        new() {
    //            State = "New York",
    //            StateCode = "NY",
    //            TodayCases = 150,
    //            TotalCases = 23456,
    //            Timestamp = DateTime.UtcNow
    //        },
    //    ]);
    //}

    public async Task<LatestStatsResponse> GetStateStatsAsync()
    {
        var result = await _client.GetFromJsonAsync<LatestStatsResponse>("/api/stats/latest");
        return result ?? new([]);
    }
}
