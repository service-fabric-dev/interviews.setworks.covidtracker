using CovidTracker.Application.Commands;
using CovidTracker.Infrastructure.Http;
using CovidTracker.Infrastructure.Messaging;
using MassTransit;

namespace CovidTracker.Application.Handlers;

public class IngestCovidStatsCommandHandler(
        CovidApiClient apiClient,
        IMessagePublisher messagePublisher
    ) : IConsumer<IngestCovidStatsCommand>
{
    private readonly CovidApiClient _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    private readonly IMessagePublisher _messagePublisher = messagePublisher ?? throw new ArgumentNullException(nameof(messagePublisher));

    public async Task Consume(ConsumeContext<IngestCovidStatsCommand> context)
    {
        var stats = await _apiClient.FetchStateStatsAsync(context.CancellationToken);
        await _messagePublisher.PublishCovidStats(stats, context.CancellationToken);
    }
}
