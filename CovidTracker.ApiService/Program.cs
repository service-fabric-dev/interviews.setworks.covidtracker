using CovidTracker.ApiService.Extensions;
using CovidTracker.Infrastructure.Extensions;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

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
builder.Services.AddApiServices();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.MapDefaultEndpoints();
app.MapControllers();

app.Run();