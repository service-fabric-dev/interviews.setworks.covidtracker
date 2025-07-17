using CovidTracker.Domain.Models;
using CovidTracker.Shared.Messages;
using MassTransit;

namespace CovidTracker.Infrastructure.Messaging;

public class MassTransitMessagePublisher(IPublishEndpoint publisher) : IMessagePublisher
{
    private readonly IPublishEndpoint _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));

    public Task PublishCovidAlert(CovidAlert alert, CancellationToken cancellation = default)
    {
        ArgumentNullException.ThrowIfNull(alert, nameof(alert));
        return _publisher.Publish(new CovidAlertMessage(alert), cancellation);
    }

    public Task PublishCovidStats(List<StateStat> stateStats, CancellationToken cancellation = default)
    {
        ArgumentNullException.ThrowIfNull(stateStats, nameof(stateStats));
        if (stateStats.Count == 0)
        {
            throw new ArgumentException("State stats list cannot be empty.", nameof(stateStats));
        }

        return _publisher.Publish(new CovidStatsMessage(stateStats), cancellation);
    }
}
