using CovidTracker.Domain.Models;

namespace CovidTracker.Application.Commands;

public record GenerateCovidAlertsCommand(List<StateStat> Stats);
