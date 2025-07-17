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
