using CovidTracker.Domain.Models;

namespace CovidTracker.Domain.Repositories;

public interface IStatSnapshotRepository
{
    Task<StateStatSnapshot> GetLatestSnapshotAsync(CancellationToken cancellation = default);
    Task SaveSnapshotAsync(StateStatSnapshot snapshot, CancellationToken cancellation = default);
}
