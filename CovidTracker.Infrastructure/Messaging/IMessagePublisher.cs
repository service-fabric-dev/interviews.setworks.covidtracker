using CovidTracker.Domain.Models;

namespace CovidTracker.Infrastructure.Messaging;

public interface IMessagePublisher
{
    Task PublishCovidStats(List<StateStat> stateStats, CancellationToken cancellation = default);
    Task PublishCovidAlert(CovidAlert alert, CancellationToken cancellation = default);
}
