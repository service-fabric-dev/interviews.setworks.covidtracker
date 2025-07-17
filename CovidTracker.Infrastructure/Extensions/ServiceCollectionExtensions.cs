using System.Reflection;

using Azure.Identity;

using CovidTracker.Application.Services;
using CovidTracker.Domain.Repositories;
using CovidTracker.Domain.Services;
using CovidTracker.Infrastructure.Data;
using CovidTracker.Infrastructure.Eventing;
using CovidTracker.Infrastructure.Http;
using CovidTracker.Infrastructure.Repositories;
using CovidTracker.Infrastructure.Services;

using MassTransit;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CovidTracker.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public record DataInfrastructureConfiguration
    {
        public required string DbConnectionString { get; init; }
    }

    public static IServiceCollection AddDataInfrastructure(
        this IServiceCollection services,
        DataInfrastructureConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentException.ThrowIfNullOrWhiteSpace(config.DbConnectionString, nameof(config.DbConnectionString));

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.DbConnectionString));

        services.AddScoped<IStatRepository, StatRepository>();
        services.AddScoped<IStatSnapshotRepository, StatSnapshotRepository>();

        return services;
    }

    public record HttpInfrastructureConfiguration
    {
        public required string CovidApiBaseUrl { get; init; }
    }

    public static IServiceCollection AddHttpInfrastructure(
        this IServiceCollection services,
        HttpInfrastructureConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentException.ThrowIfNullOrWhiteSpace(config.CovidApiBaseUrl, nameof(config.CovidApiBaseUrl));

        services.AddHttpClient<CovidApiClient>(client =>
        {
            client.BaseAddress = new Uri(config.CovidApiBaseUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        });

        services.AddScoped<IStatIngestionService, StatIngestionService>();

        return services;
    }

    public record EventingInfrastructureConfiguration
    {
        public required string BusConnectionString { get; init; }
        public Assembly? ConsumerAssembly { get; init; }
        public IEnumerable<Assembly>? CqrsAssemblies { get; init; }
        public bool UsePresentationEvents { get; init; } = false;
    }

    // TODO: separate this by event type (e.g., domain, integration, presentation)
    public static IServiceCollection AddEventingInfrastructure(
        this IServiceCollection services,
        EventingInfrastructureConfiguration config)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));
        ArgumentException.ThrowIfNullOrWhiteSpace(config.BusConnectionString, nameof(config.BusConnectionString));

        services.AddMassTransit(cfg =>
        {
            // Command/query consumers
            if (config.CqrsAssemblies != null)
            {
                cfg.AddMediator(medCfg =>
                {
                    medCfg.AddConsumers([.. config.CqrsAssemblies]);
                });
            }

            // Transport-based consumers
            if (config.ConsumerAssembly != null)
            {
                cfg.AddConsumers(config.ConsumerAssembly);
            }

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


        if (!config.UsePresentationEvents)
        {
            services.AddScoped<IDomainPublisherService, DomainPublisherService>();
            services.AddScoped<IIntegrationPublisherService, IntegrationPublisherService>();
        }
        else
        {
            services.AddScoped<IPresentationPublisherService, PresentationPublisherService>();
        }

        return services;
    }
}
