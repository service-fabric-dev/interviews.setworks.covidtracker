using CovidTracker.Infrastructure.Extensions;
using CovidTracker.WorkerService.Workers;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.Services
    .AddDataInfrastructure(new()
    {
        DbConnectionString = builder.Configuration.GetConnectionString("CovidTrackerDb") ?? throw new InvalidOperationException("Missing connection string 'CovidTrackerDb'"),
    })
    .AddHttpInfrastructure(new()
    {
        CovidApiBaseUrl = "https://disease.sh/v3/covid-19/"
    })
    .AddEventingInfrastructure(new()
{
    BusConnectionString = builder.Configuration.GetConnectionString("CovidTrackerBus") ?? throw new InvalidOperationException("Missing connection string 'CovidTrackerBus'"),
    CqrsAssemblies = [
        typeof(CovidTracker.Application.Commands.GenerateCovidAlertsCommand).Assembly,
    ]
});

builder.Services.AddHostedService<StatsIngestionWorker>();

var host = builder.Build();
host.Run();
