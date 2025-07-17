using CovidTracker.ApiService.Extensions;
using CovidTracker.ApiService.Hubs;
using CovidTracker.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddSignalR();

// TODO: update connection string in appsettings.json to reference Azure SQL Database
builder.Services.AddInfrastructureServices(new()
{
    DbConnectionString = builder.Configuration.GetConnectionString("CovidTrackerDb") ?? throw new InvalidOperationException("Missing connection string 'DefaultConnection'"),
    BusConnectionString = builder.Configuration.GetConnectionString("CovidTrackerBus") ?? throw new InvalidOperationException("Missing connection string 'BusConnection'"),
    ConsumerAssembly = typeof(CovidTracker.ApiService.Consumers.CovidAlertConsumer).Assembly,
    MediatorAssemblies = [
        typeof(CovidTracker.ApiService.Consumers.CovidAlertConsumer).Assembly,
        typeof(CovidTracker.Application.Commands.GenerateCovidAlertsCommand).Assembly,
    ]
});

builder.Services.AddApiServices();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapHub<CovidHub>("hubs/covid");

app.MapDefaultEndpoints();

app.MapControllers();

app.Run();