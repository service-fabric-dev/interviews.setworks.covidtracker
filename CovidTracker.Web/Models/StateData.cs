using System.Text.Json.Serialization;

namespace CovidTracker.Web.Models;

public record StateData(string name, string abbreviation, string description);

public record ShapeDataObject
{
    [JsonPropertyName("type")]
    public string? type { get; set; }

    [JsonPropertyName("features")]
    public List<Feature> features { get; set; } = [];
}

public record Feature
{
    [JsonPropertyName("type")]
    public string? type { get; set; }

    [JsonPropertyName("properties")]
    public PropertiesObject? properties { get; set; }

    [JsonPropertyName("geometry")]
    public Geometry? geometry { get; set; }
}

public record PropertiesObject
{
    [JsonPropertyName("name")]
    public string? name { get; set; }

    [JsonPropertyName("abbreviation")]
    public string? abbreviation { get; set; }
}

public record Geometry
{
    [JsonPropertyName("type")]
    public string? type { get; set; }


    [JsonPropertyName("coordinates")]
    public List<List<List<double>>> coordinates { get; set; } = [];
}