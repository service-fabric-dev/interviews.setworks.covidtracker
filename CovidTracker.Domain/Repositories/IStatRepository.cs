using CovidTracker.Domain.Models;

namespace CovidTracker.Domain.Repositories;

public interface IStatRepository
{
    Task UpsertStateStatsAsync(IEnumerable<StateStat> stateStats, CancellationToken cancellation = default);
    Task<List<StateStat>> GetLatestStatsAsync(CancellationToken cancellation = default);
    Task<List<List<StateStat>>> GetLatestStatsAsync(int periods, CancellationToken cancellation = default);
    Task SaveAlertAsync(CovidAlert alert, CancellationToken cancellation = default);
    Task SaveAlertsAsync(IEnumerable<CovidAlert> alert, CancellationToken cancellation = default);
    Task<List<CovidAlert>> GetRecentAlertsAsync(CancellationToken cancellation = default);
}
