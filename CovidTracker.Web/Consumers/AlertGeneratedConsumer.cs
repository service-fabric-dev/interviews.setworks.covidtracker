using CovidTracker.Application.Services;
using CovidTracker.Domain.Events;

using MassTransit;

namespace CovidTracker.Web.Consumers;

/// <summary>
/// Consumer that broadcasts generated COVID-19 alerts to all connected clients via SignalR.
/// </summary>
/// <param name="publisher">Presentation event publisher to forward events to the client UI</param>
public class AlertGeneratedConsumer(
        IPresentationPublisherService publisher
    ) : IConsumer<AlertGeneratedEvent>
{
    private readonly IPresentationPublisherService _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));

    public async Task Consume(ConsumeContext<AlertGeneratedEvent> context)
    {
        var alert = context.Message.Alert;
        await _publisher.PublishEvent("ReceiveAlert", alert, context.CancellationToken);
    }
}
