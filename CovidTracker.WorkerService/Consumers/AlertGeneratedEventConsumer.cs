using CovidTracker.Domain.Events;

using MassTransit;

namespace CovidTracker.WorkerService.Consumers;

/// <summary>
/// Consumer for the AlertGeneratedEvent integration event that persists the alert to the database.
/// </summary>
public class AlertGeneratedEventConsumer(
        
    ) : IConsumer<AlertGeneratedEvent>
{
    public async Task Consume(ConsumeContext<AlertGeneratedEvent> context)
    {

    }
}
