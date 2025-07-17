using CovidTracker.Infrastructure.Extensions;
using CovidTracker.Infrastructure.Hubs;
using CovidTracker.Web.Components;
using CovidTracker.Web.Services;

using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);


// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();
builder.AddRedisOutputCache("cache");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddLogging();

builder.Services
    .AddEventingInfrastructure(new()
    {
        UsePresentationEvents = true,
        BusConnectionString = builder.Configuration.GetConnectionString("CovidTrackerBus") ?? throw new InvalidOperationException("Missing connection string 'CovidTrackerBus'"),
        ConsumerAssembly = typeof(CovidTracker.Web.Consumers.AlertGeneratedConsumer).Assembly
    });

builder.Services.AddHttpClient<ApiService>((sp, client) =>
{
    var baseUri = sp.GetRequiredService<IConfiguration>()["services:apiservice:https:0"]
                    ?? throw new InvalidOperationException("Missing configuration for 'services:apiservice:https:0'");
    client.BaseAddress = new Uri(baseUri);
});

builder.Services.AddSignalR();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        ["application/octet-stream"]);
});

var app = builder.Build();

app.UseResponseCompression();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.MapHub<AlertsHub>("/alerts");

app.Run();
