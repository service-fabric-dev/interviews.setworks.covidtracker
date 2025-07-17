using CovidTracker.Domain.Models;

namespace CovidTracker.Domain.Services;

public interface IStatIngestionService
{
    Task<List<StateStat>> FetchCurrentStatsAsync(CancellationToken cancellation = default);
}
