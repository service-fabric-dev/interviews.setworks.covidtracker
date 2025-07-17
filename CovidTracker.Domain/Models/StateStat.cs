namespace CovidTracker.Domain.Models;

public class StateStat
{
    public string State { get; set; }
    public string StateCode { get; set; }
    public int TodayCases { get; set; }
    public int TotalCases { get; set; }
    public DateTime Timestamp { get; set; }
}
