using CovidTracker.Domain.Models;

namespace CovidTracker.Application.Commands;

public record SaveCovidAlertsCommand(List<CovidAlert> Alerts);
