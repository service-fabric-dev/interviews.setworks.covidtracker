using CovidTracker.Domain.Models;

namespace CovidTracker.Shared.DTOs;

public record LatestStatsResponse(List<StateStat> Stats);
