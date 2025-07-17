using Microsoft.AspNetCore.SignalR;
using MassTransit;
using MassTransit.Mediator;
using CovidTracker.ApiService.Hubs;
using CovidTracker.Application.Commands;
using CovidTracker.Shared.Messages;

namespace CovidTracker.ApiService.Consumers;

/// <summary>
/// Consumer that saves stats to the database and broadcasts updates via SignalR to the Blazor UI.
/// </summary>
/// <param name="mediator">Command/Query event mediator</param>
/// <param name="hub">Hub for SignalR messages</param>
public class CovidStatsConsumer(
        IMediator mediator, 
        IHubContext<CovidHub> hub
    ) : IConsumer<CovidStatsMessage>
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    private readonly IHubContext<CovidHub> _hub = hub ?? throw new ArgumentNullException(nameof(hub));

    public async Task Consume(ConsumeContext<CovidStatsMessage> context)
    {
        var stats = context.Message.Stats;
        await _mediator.Send(new UpsertCovidStatsCommand(stats), context.CancellationToken);

        var summary = new
        {
            TotalCases = stats.Sum(s => s.TodayCases),
            Timestamp = DateTime.UtcNow
        };

        await _hub.Clients.All.SendAsync("ReceiveCovidUpdate", summary, context.CancellationToken);
    }
}
