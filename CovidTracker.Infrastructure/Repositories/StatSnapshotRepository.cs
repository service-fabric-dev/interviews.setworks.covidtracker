using CovidTracker.Domain.Models;
using CovidTracker.Domain.Repositories;

namespace CovidTracker.Infrastructure.Repositories;

public class StatSnapshotRepository(IStatRepository statRepository) : IStatSnapshotRepository
{
    private readonly IStatRepository _statRepository = statRepository ?? throw new ArgumentNullException(nameof(statRepository));

    public async Task<StateStatSnapshot> GetLatestSnapshotAsync(CancellationToken cancellation = default)
    {
        var stats = await _statRepository.GetLatestStatsAsync(cancellation);
        return new StateStatSnapshot(stats);
    }

    public async Task SaveSnapshotAsync(StateStatSnapshot snapshot, CancellationToken cancellation = default)
    {
        await _statRepository.UpsertStateStatsAsync(snapshot.Stats, cancellation);
    }
}
