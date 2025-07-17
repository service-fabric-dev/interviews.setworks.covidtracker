
using CovidTracker.Domain.Services;

using MassTransit.Mediator;

namespace CovidTracker.Infrastructure.Eventing;

/// <summary>
/// Service for publishing in-process domain events using MassTransit Mediator.
/// </summary>
/// <param name="mediator">Mediator interface from MassTransit for publishing in-process events</param>
public class DomainPublisherService(IMediator mediator) : IDomainPublisherService
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    public Task PublishEvent<TEvent>(TEvent @event, CancellationToken cancellation = default) where TEvent : class
    {
        ArgumentNullException.ThrowIfNull(@event, nameof(@event));

        return _mediator.Publish(@event, cancellation);
    }
}
