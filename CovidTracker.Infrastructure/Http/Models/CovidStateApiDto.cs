namespace CovidTracker.Infrastructure.Http.Models;

public record CovidStateApiDto
{
    public required string State { get; set; }
    public long Updated { get; set; }
    public int Cases { get; set; }
    public int TodayCases { get; set; }
    public int Deaths { get; set; }
    public int TodayDeaths { get; set; }
    public int Population { get; set; }
    public int Recovered { get; set; }
    public int CasesPerOneMillion { get; set; }
    public int DeathsPerOneMillion { get; set; }
}
