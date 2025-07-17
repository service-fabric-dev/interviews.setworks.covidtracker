using CovidTracker.Application.Commands;
using CovidTracker.Domain.Services;

namespace CovidTracker.WorkerService.Workers;

/// <summary>
/// Background worker that periodically ingests COVID-19 statistics.
/// </summary>
/// <param name="scopeFactory">Factory for manufacturing scoped <see cref="IDomainPublisherService"/> instances</param>
public class StatsIngestionWorker(
        IServiceScopeFactory scopeFactory
    ) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellation)
    {
        while (!cancellation.IsCancellationRequested)
        {
            var scope = scopeFactory.CreateScope();
            var publisher = scope.ServiceProvider.GetRequiredService<IDomainPublisherService>();

            await publisher.PublishEvent(new IngestStatsCommand(), cancellation);
            await Task.Delay(TimeSpan.FromMinutes(100), cancellation);
        }
    }
}
