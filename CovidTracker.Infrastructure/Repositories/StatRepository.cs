using CovidTracker.Domain.Models;
using CovidTracker.Domain.Repositories;
using CovidTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CovidTracker.Infrastructure.Repositories;

public class StatRepository(AppDbContext dbContext) : IStatRepository
{
    private readonly AppDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<List<StateStat>> GetLatestStatsAsync(CancellationToken cancellation = default)
    {
        var query = @"
            SELECT s.*
            FROM StateStats s
            INNER JOIN (
                SELECT State, MAX(Timestamp) AS LatestTimestamp
                FROM StateStats
                GROUP BY State
            ) latest
            ON s.State = latest.State AND s.Timestamp = latest.LatestTimestamp
        ";

        var result = await _dbContext.StateStats
            .FromSqlRaw(query)
            .AsNoTracking()
            .ToListAsync(cancellation);

        return result;
    }

    public async Task<List<List<StateStat>>> GetLatestStatsAsync(int periods, CancellationToken cancellation = default)
    {
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(periods, 0, nameof(periods));

        var query = @"
            SELECT s.*, latest.rn
            FROM StateStats s
            INNER JOIN (
                SELECT State, Timestamp, ROW_NUMBER() OVER (PARTITION BY State ORDER BY Timestamp DESC) AS rn
                FROM StateStats
            ) latest
            ON s.State = latest.State AND s.Timestamp = latest.Timestamp
            WHERE latest.rn <= {0}
        ";

        var result = await _dbContext.StateStats
            .FromSqlRaw(query, periods)
            .AsNoTracking()
            .ToListAsync(cancellation);

        // Group by rn (snapshot index), then for each snapshot, get all states
        var snapshots = result
            .GroupBy(stat => EF.Property<int>(stat, "rn"))
            .OrderBy(g => g.Key)
            .Select(g => g.ToList())
            .ToList();

        return snapshots;
    }

    public async Task<List<CovidAlert>> GetRecentAlertsAsync(CancellationToken cancellation = default)
    {
        return await _dbContext.CovidAlerts
            .AsNoTracking()
            .OrderByDescending(alert => alert.Time)
            .Take(100)
            .ToListAsync(cancellation);
    }

    public async Task SaveAlertAsync(CovidAlert alert, CancellationToken cancellation = default)
    {
        ArgumentNullException.ThrowIfNull(alert, nameof(alert));

        var existing = await _dbContext.CovidAlerts
            .FindAsync([alert.State, alert.Time], cancellation);
        if (existing == null)
        {
            _dbContext.CovidAlerts.Add(alert);
        }
        else
        {
            existing.Message = alert.Message;
            existing.Severity = alert.Severity;
            _dbContext.CovidAlerts.Update(existing);
        }
        await _dbContext.SaveChangesAsync(cancellation);
    }

    public async Task SaveAlertsAsync(IEnumerable<CovidAlert> alerts, CancellationToken cancellation = default)
    {
        ArgumentNullException.ThrowIfNull(alerts, nameof(alerts));
        if (!alerts.Any())
        {
            return; // No alerts to process
        }

        await Parallel.ForEachAsync(alerts, cancellation, async (alert, ct) =>
        {
            ArgumentNullException.ThrowIfNull(alert, nameof(alert));
            var existing = await _dbContext.CovidAlerts
                .FindAsync([alert.State, alert.Time], cancellationToken: cancellation);
            if (existing == null)
            {
                _dbContext.CovidAlerts.Add(alert);
            }
            else
            {
                existing.Message = alert.Message;
                existing.Severity = alert.Severity;
                _dbContext.CovidAlerts.Update(existing);
            }
        });
        await _dbContext.SaveChangesAsync(cancellation);
    }

    public async Task UpsertStateStatsAsync(IEnumerable<StateStat> stats, CancellationToken cancellation = default)
    {
        foreach (var stat in stats)
        {
            var existing = await _dbContext.StateStats
                .FindAsync([stat.State, stat.Timestamp], cancellationToken: cancellation);

            if (existing == null)
            {
                _dbContext.StateStats.Add(stat);
            }
            else
            {
                _dbContext.StateStats.Update(existing);
            }
        }

        await _dbContext.SaveChangesAsync(cancellation);
    }
}
