using CovidTracker.Application.Services;
using CovidTracker.Domain.Events;

using MassTransit;

namespace CovidTracker.Web.Consumers;

/// <summary>
/// Consumer that saves stats to the database and broadcasts updates via SignalR to the Blazor UI.
/// </summary>
/// <param name="publisher">Presentation event publisher to forward events to the client UI</param>
public class StatsIngestedConsumer(
        IPresentationPublisherService publisher
    ) : IConsumer<StatsIngestedEvent>
{
    private readonly IPresentationPublisherService _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));

    public async Task Consume(ConsumeContext<StatsIngestedEvent> context)
    {
        var stats = context.Message.Stats;
        await _publisher.PublishEvent("ReceiveCovidUpdate", stats, context.CancellationToken);
    }
}
