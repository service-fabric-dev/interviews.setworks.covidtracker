using CovidTracker.Domain.Models;

namespace CovidTracker.Domain.Events;

/// <summary>
/// An integration event that is published when a new COVID-19 alert is generated.
/// </summary>
/// <param name="Alert">The generated alert</param>
public record AlertGeneratedEvent(CovidAlert Alert);
