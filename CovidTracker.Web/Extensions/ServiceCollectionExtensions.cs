using CovidTracker.Web.Services;

namespace CovidTracker.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddHttpClient<ApiService>((sp, client) =>
        {
            var baseUri = sp.GetRequiredService<IConfiguration>()["services:apiservice:https:0"]
                            ?? throw new InvalidOperationException("Missing configuration for 'services:apiservice:https:0'");
            client.BaseAddress = new Uri(baseUri);
        });

        return services;
    }
}
