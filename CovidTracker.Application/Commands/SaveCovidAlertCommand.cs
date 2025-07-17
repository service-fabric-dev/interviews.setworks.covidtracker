using CovidTracker.Domain.Models;

namespace CovidTracker.Application.Commands;

public record SaveCovidAlertCommand(CovidAlert Alert);
