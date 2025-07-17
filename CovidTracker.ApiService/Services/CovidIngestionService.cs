using CovidTracker.Application.Commands;
using MassTransit.Mediator;

namespace CovidTracker.ApiService.Services;

public class CovidIngestionService(
        IMediator mediator
    ) : BackgroundService
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _mediator.Send(new IngestCovidStatsCommand(), stoppingToken);
            await Task.Delay(TimeSpan.FromMinutes(100), stoppingToken);
        }
    }
}
