using CovidTracker.Domain.Models;

namespace CovidTracker.Shared.Messages;

public record CovidStatsMessage(List<StateStat> Stats)
{
}
