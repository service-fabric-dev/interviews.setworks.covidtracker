using CovidTracker.Web.Models;
using CovidTracker.Web.Services;
using Microsoft.AspNetCore.Components;
using System.Text.Json;

namespace CovidTracker.Web.Components.Maps;

public partial class CovidMapSf
{
    [Inject] private ILogger<CovidMapSf> Logger { get; set; } = default!;
    [Inject] private NavigationManager NavManager { get; set; } = default!;
    [Inject] private ApiService ApiService { get; set; } = default!;
    [Inject] private HttpClient Http { get; set; } = default!;

    public object? ShapeData { get; set; }
    public List<StateData> StateInfo { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        var baseUri = NavManager.BaseUri;
        var shapeDataString = await Http.GetStringAsync($"{baseUri}maps/us-states.json");
        if (!string.IsNullOrWhiteSpace(shapeDataString))
        {
            Logger.LogDebug("Successfully loaded shape data: {Preview}", shapeDataString[..100]);
        }
        else
        {
            Logger.LogError("Failed to load shape data!");
        }

        ShapeData = JsonSerializer.Deserialize<object>(shapeDataString);
        if (ShapeData != null)
        {
            Logger.LogDebug("Successfully deserialized shape data");
        }
        else
        {
            Logger.LogError("Failed to deserialize shape data!");
        }

        // Load dynamic state data from your service
        StateInfo = await ApiService.GetStateDataAsync();
    }
}
