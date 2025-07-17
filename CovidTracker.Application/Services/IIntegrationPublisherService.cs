namespace CovidTracker.Application.Services;

/// <summary>
/// Interface for a service that publishes integration events.
/// </summary>
public interface IIntegrationPublisherService
{
    /// <summary>
    /// Publishes an integration event to a cross-service message bus.
    /// </summary>
    /// <typeparam name="TEvent">Type of event to publish</typeparam>
    /// <param name="event">The event to publish</param>
    /// <param name="cancellation">Cancellation support</param>
    /// <returns>An awaitable task</returns>
    Task PublishEvent<TEvent>(TEvent @event, CancellationToken cancellation = default)
        where TEvent : class;
}
