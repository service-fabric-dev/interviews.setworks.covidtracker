using CovidTracker.Web.Components;
using CovidTracker.Web.Services;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);


// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();
builder.AddRedisOutputCache("cache");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddLogging();

builder.Services.AddHttpClient<ApiService>((sp, client) =>
{
    var baseUri = sp.GetRequiredService<IConfiguration>()["services:apiservice:https:0"]
                    ?? throw new InvalidOperationException("Missing configuration for 'services:apiservice:https:0'");
    client.BaseAddress = new Uri(baseUri);
});

builder.Services.AddSyncfusionBlazor();
//builder.Services.AddScoped<ApiService>();

var app = builder.Build();

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

app.Run();
