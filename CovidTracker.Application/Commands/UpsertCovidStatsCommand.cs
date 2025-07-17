using CovidTracker.Domain.Models;

namespace CovidTracker.Application.Commands;

public record UpsertCovidStatsCommand(List<StateStat> Stats);
