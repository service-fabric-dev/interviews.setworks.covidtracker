using CovidTracker.Application.Services;
using CovidTracker.Domain.Events;
using CovidTracker.Domain.Models;

using MassTransit;

namespace CovidTracker.Infrastructure.Eventing;

public class IntegrationPublisherService(IPublishEndpoint publisher) : IIntegrationPublisherService
{
    private readonly IPublishEndpoint _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));

    public Task PublishCovidAlert(CovidAlert alert, CancellationToken cancellation = default)
    {
        ArgumentNullException.ThrowIfNull(alert, nameof(alert));
        return _publisher.Publish(new AlertGeneratedEvent(alert), cancellation);
    }

    public Task PublishCovidStats(List<StateStat> stateStats, CancellationToken cancellation = default)
    {
        ArgumentNullException.ThrowIfNull(stateStats, nameof(stateStats));
        if (stateStats.Count == 0)
        {
            throw new ArgumentException("State stats list cannot be empty.", nameof(stateStats));
        }

        return _publisher.Publish(new StatsIngestedEvent(stateStats), cancellation);
    }

    public Task PublishEvent<TEvent>(TEvent @event, CancellationToken cancellation = default) where TEvent : class
    {
        ArgumentNullException.ThrowIfNull(@event, nameof(@event));

        return _publisher.Publish(@event, cancellation);
    }
}
