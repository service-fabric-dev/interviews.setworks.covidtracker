using System.Text.Json.Serialization;

namespace CovidTracker.Domain.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CovidAlertSeverity
{
    Low,    // e.g. "Low"
    Medium, // e.g. "Medium"
    High,   // e.g. "High"
    Critical // e.g. "Critical"
}

public class CovidAlert
{
    public string State { get; set; }
    public string Message { get; set; }
    public CovidAlertSeverity Severity { get; set; }
    public DateTime Time { get; set; }
}