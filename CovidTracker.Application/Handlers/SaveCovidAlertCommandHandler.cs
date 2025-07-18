using CovidTracker.Application.Commands;
using CovidTracker.Domain.Repositories;

using MassTransit;

namespace CovidTracker.Application.Handlers;

/// <summary>
/// CQRS command handler that saves a COVID-19 alert.
/// </summary>
/// <param name="statRepository"></param>
public class SaveCovidAlertCommandHandler(
        IStatRepository statRepository
    ) : IConsumer<SaveCovidAlertsCommand>
{
    private readonly IStatRepository _statRepository = statRepository ?? throw new ArgumentNullException(nameof(statRepository));

    public async Task Consume(ConsumeContext<SaveCovidAlertsCommand> context)
    {
        await _statRepository.SaveAlertsAsync(context.Message.Alerts, context.CancellationToken);
    }
}
