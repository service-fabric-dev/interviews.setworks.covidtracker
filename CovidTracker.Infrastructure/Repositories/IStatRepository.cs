using CovidTracker.Domain.Models;

namespace CovidTracker.Infrastructure.Repositories;

public interface IStatRepository
{
    Task UpsertStateStatsAsync(IEnumerable<StateStat> stateStats, CancellationToken cancellation = default);
    Task<List<StateStat>> GetLatestStatsAsync(CancellationToken cancellation = default);
    Task SaveAlertAsync(CovidAlert alert, CancellationToken cancellation = default);
    Task<List<CovidAlert>> GetRecentAlertsAsync(CancellationToken cancellation = default);
}
