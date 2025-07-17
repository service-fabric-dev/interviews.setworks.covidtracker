using CovidTracker.ApiService.Services;

namespace CovidTracker.ApiService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddHostedService<CovidIngestionService>();

        return services;
    }
}
