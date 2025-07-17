using MassTransit;
using MassTransit.Mediator;
using CovidTracker.Application.Commands;
using CovidTracker.Shared.Messages;

namespace CovidTracker.ApiService.Consumers;

/// <summary>
/// Consumer that checks for significant increases in COVID-19 cases and publishes alerts.
/// </summary>
/// <param name="mediator">Command/Query event mediator</param>
public class CovidStatsAlertConsumer(
        IMediator mediator
    ) : IConsumer<CovidStatsMessage>
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    public async Task Consume(ConsumeContext<CovidStatsMessage> context)
    {
        var stats = context.Message.Stats;
        await _mediator.Publish(new GenerateCovidAlertsCommand(stats));
    }
}
