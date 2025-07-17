namespace CovidTracker.Application.Services;

/// <summary>
/// Interface for a service that publishes presentation events.
/// </summary>
public interface IPresentationPublisherService
{
    /// <summary>
    /// Publishes a presentation event to a websocket-style message bus for consumption in application UI
    /// </summary>
    /// <typeparam name="TEvent">Type of event to publish</typeparam>
    /// <param name="eventName">Name of the event to publish</param>
    /// <param name="event">The event to publish</param>
    /// <param name="cancellation">Cancellation support</param>
    /// <returns>An awaitable task</returns>
    Task PublishEvent<TEvent>(string eventName, TEvent @event, CancellationToken cancellation = default)
        where TEvent : class;
}
