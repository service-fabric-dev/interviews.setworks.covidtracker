using CovidTracker.Application.Commands;
using CovidTracker.Domain.Repositories;

using MassTransit;

namespace CovidTracker.Application.Handlers;

public class UpsertStatsCommandHandler(
        IStatRepository statRepository
    ) : IConsumer<UpsertCovidStatsCommand>
{
    private readonly IStatRepository _statRepository = statRepository;

    public async Task Consume(ConsumeContext<UpsertCovidStatsCommand> context)
    {
        await _statRepository.UpsertStateStatsAsync(context.Message.Stats, context.CancellationToken);
    }
}
