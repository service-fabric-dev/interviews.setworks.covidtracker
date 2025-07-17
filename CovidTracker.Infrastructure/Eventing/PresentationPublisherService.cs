using CovidTracker.Application.Services;
using CovidTracker.Infrastructure.Hubs;

using Microsoft.AspNetCore.SignalR;

namespace CovidTracker.Infrastructure.Eventing;

/// <summary>
/// Service for publishing in-process presentation events using a SignalR hub.
/// </summary>
/// <param name="hub">Hub interface from SignalR for publishing events</param>
public class PresentationPublisherService(IHubContext<AlertsHub> hub) : IPresentationPublisherService
{
    private readonly IHubContext<AlertsHub> _hub = hub ?? throw new ArgumentNullException(nameof(hub));

    public Task PublishEvent<TEvent>(string eventName, TEvent @event, CancellationToken cancellation = default) where TEvent : class
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(eventName, nameof(eventName));
        ArgumentNullException.ThrowIfNull(@event, nameof(@event));

        return _hub.Clients.All.SendAsync(eventName, @event, cancellation);
    }
}
