using Azure.Identity;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using CovidTracker.Infrastructure.Data;
using CovidTracker.Infrastructure.Http;
using CovidTracker.Infrastructure.Messaging;
using CovidTracker.Infrastructure.Repositories;

namespace CovidTracker.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public record InfrastructureServiceConfiguration
    {
        public required string DbConnectionString { get; init; }
        public required string BusConnectionString { get; init; }
        public required Assembly ConsumerAssembly { get; init; }
        public required IEnumerable<Assembly> MediatorAssemblies { get; init; }
    }

    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        InfrastructureServiceConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentException.ThrowIfNullOrWhiteSpace(config.DbConnectionString, nameof(config.DbConnectionString));
        ArgumentException.ThrowIfNullOrWhiteSpace(config.BusConnectionString, nameof(config.BusConnectionString));
        ArgumentNullException.ThrowIfNull(config.ConsumerAssembly, nameof(config.ConsumerAssembly));

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.DbConnectionString));

        services.AddScoped<IStatRepository, StatRepository>();

        services.AddHttpClient<CovidApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://disease.sh/v3/covid-19/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        });

        services.AddMassTransit(cfg =>
        {
            // Command/handler consumers
            cfg.AddMediator(medCfg =>
            {
                medCfg.AddConsumers([.. config.MediatorAssemblies]);
            });

            // Transport-based consumers
            cfg.AddConsumers(config.ConsumerAssembly);

            cfg.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Host(config.BusConnectionString, hostSettings =>
                {
                    hostSettings.TokenCredential = new DefaultAzureCredential();
                });
                cfg.ConfigureEndpoints(context);
                cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(10)));
            });
        });
        services.AddScoped<IMessagePublisher, MassTransitMessagePublisher>();

        return services;
    }
}
