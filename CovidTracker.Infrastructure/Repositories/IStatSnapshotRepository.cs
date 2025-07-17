using CovidTracker.Domain.Models;

namespace CovidTracker.Infrastructure.Repositories;

public interface IStatSnapshotRepository
{
    Task<StateStatSnapshot> GetLatestSnapshotAsync(CancellationToken cancellation = default);
    Task SaveSnapshotAsync(StateStatSnapshot snapshot, CancellationToken cancellation = default);
}
