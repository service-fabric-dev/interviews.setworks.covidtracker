using CovidTracker.Application.Services;
using CovidTracker.Domain.Events;

using MassTransit;

namespace CovidTracker.Web.Consumers;

/// <summary>
/// Consumer that broadcasts updates via SignalR to the Blazor UI.
/// </summary>
/// <param name="presentationPublisher">Presentation event publisher to forward events to the client UI</param>
public class StatsIngestedConsumer(
        IPresentationPublisherService presentationPublisher
    ) : IConsumer<StatsIngestedEvent>
{
    private readonly IPresentationPublisherService _presentationPublisher = presentationPublisher ?? throw new ArgumentNullException(nameof(presentationPublisher));

    public async Task Consume(ConsumeContext<StatsIngestedEvent> context)
    {
        var stats = context.Message.Stats;
        await _presentationPublisher.PublishEvent("ReceiveCovidUpdate", stats, context.CancellationToken);
    }
}
