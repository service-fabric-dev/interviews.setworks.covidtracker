using CovidTracker.Application.Commands;
using CovidTracker.Infrastructure.Repositories;
using MassTransit;

namespace CovidTracker.Application.Handlers;

public class SaveCovidAlertCommandHandler(
        IStatRepository statRepository
    ) : IConsumer<SaveCovidAlertCommand>
{
    private readonly IStatRepository _statRepository = statRepository ?? throw new ArgumentNullException(nameof(statRepository));

    public async Task Consume(ConsumeContext<SaveCovidAlertCommand> context)
    {
        await _statRepository.SaveAlertAsync(context.Message.Alert, context.CancellationToken);
    }
}
