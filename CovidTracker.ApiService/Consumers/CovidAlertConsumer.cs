using Microsoft.AspNetCore.SignalR;
using MassTransit;
using MassTransit.Mediator;
using CovidTracker.Shared.Messages;
using CovidTracker.ApiService.Hubs;
using CovidTracker.Application.Commands;

namespace CovidTracker.ApiService.Consumers;

/// <summary>
/// Consumer that saves alerts to the database and broadcasts them via SignalR to the Blazor UI.
/// </summary>
/// <param name="mediator">Command/Query event mediator</param>
/// <param name="hub">SignalR hub for publishing events to client</param>
public class CovidAlertConsumer(
        IMediator mediator,
        IHubContext<CovidHub> hub
    ) : IConsumer<CovidAlertMessage>
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    private readonly IHubContext<CovidHub> _hub = hub ?? throw new ArgumentNullException(nameof(hub));

    public async Task Consume(ConsumeContext<CovidAlertMessage> context)
    {
        var alert = context.Message.Alert;
        await _mediator.Send(new SaveCovidAlertCommand(alert), context.CancellationToken);
        await _hub.Clients.All.SendAsync("ReceiveCovidAlert", alert, context.CancellationToken);
    }
}
