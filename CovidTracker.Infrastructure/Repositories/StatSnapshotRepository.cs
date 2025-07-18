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

    public async Task<(StateStatSnapshot previous, StateStatSnapshot current)> GetLastTwoSnapshotsAsync(CancellationToken cancellation = default)
    {
        var stateGroups = await _statRepository.GetLatestStatsAsync(2, cancellation);
        if (stateGroups.Count < 2)
        {
            throw new InvalidOperationException("Not enough snapshots available to compare.");
        }

        return ( // TODO: make sure this is not inverted
            new StateStatSnapshot(stateGroups[0]),
            new StateStatSnapshot(stateGroups[1])
        );
    }

    public async Task SaveSnapshotAsync(StateStatSnapshot snapshot, CancellationToken cancellation = default)
    {
        await _statRepository.UpsertStateStatsAsync(snapshot.Stats, cancellation);
    }
}
