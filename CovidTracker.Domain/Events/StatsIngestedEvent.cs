using CovidTracker.Domain.Models;

namespace CovidTracker.Domain.Events;

/// <summary>
/// An integration event that is published after new COVID-19 statistics have been ingested.
/// </summary>
/// <param name="Stats">The recently ingested stats</param>
public record StatsIngestedEvent(List<StateStat> Stats);
