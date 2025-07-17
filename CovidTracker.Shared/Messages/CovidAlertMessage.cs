using CovidTracker.Domain.Models;

namespace CovidTracker.Shared.Messages;

public record CovidAlertMessage(CovidAlert Alert)
{
}
