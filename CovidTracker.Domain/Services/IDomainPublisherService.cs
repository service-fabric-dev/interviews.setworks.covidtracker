namespace CovidTracker.Domain.Services;

/// <summary>
/// Interface for a service that publishes domain events.
/// </summary>
public interface IDomainPublisherService
{
    /// <summary>
    /// Publishes a domain event to the in-process message bus.
    /// </summary>
    /// <typeparam name="TEvent">Type of the event to publish</typeparam>
    /// <param name="event">The event to publish</param>
    /// <param name="cancellation">Cancellation support</param>
    /// <returns>An awaitable task</returns>
    Task PublishEvent<TEvent>(TEvent @event, CancellationToken cancellation = default)
        where TEvent : class;
}
