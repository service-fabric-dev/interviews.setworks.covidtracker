namespace CovidTracker.Domain.Models;

public class StateStatSnapshot(IEnumerable<StateStat> stats)
{
    private DateTime Timestamp { get; } = DateTime.UtcNow;

    private double Threshold { get; } = 0.5;

    private readonly Dictionary<string, StateStat> _stats = stats?.ToDictionary(s => s.State, StringComparer.OrdinalIgnoreCase) 
        ?? throw new ArgumentNullException(nameof(stats), "State stats cannot be null");

    public IEnumerable<CovidAlert> GenerateAlerts(StateStatSnapshot previous)
    {
        foreach (var (state, current) in _stats)
        {
            if (!previous.TryGetStateStat(state, out var prev) || prev.TodayCases == 0)
            {
                continue; // No previous data or no cases to compare
            }

            var delta = (double)(current.TodayCases - prev.TodayCases) / prev.TodayCases;
            if (delta > Threshold)
            {
                yield return new CovidAlert
                {
                    State = state,
                    Message = $"{state} cases increased by {delta:P0} since last report.",
                    Severity = CovidAlertSeverity.High,
                    Time = Timestamp
                };
            }
        }
    }

    public IReadOnlyCollection<StateStat> Stats => [.. _stats.Values];

    public IReadOnlyDictionary<string, StateStat> StatsByState => _stats;

    public bool TryGetStateStat(string state, out StateStat stat)
    {
        return _stats.TryGetValue(state, out stat);
    }
}
