
using CovidTracker.Application.Queries;
using CovidTracker.Shared.DTOs;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace CovidTracker.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatsController(
        IMediator mediator,
        ILogger<StatsController> logger
    ) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    private readonly ILogger<StatsController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    [HttpGet("latest")]
    public async Task<IActionResult> GetLatestStats(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Received request to get latest COVID stats");

        var client = _mediator.CreateRequestClient<GetLatestStatsQuery>();
        var query = new GetLatestStatsQuery();
        var response = await client.GetResponse<LatestStatsResponse>(query, cancellationToken);
        return Ok(response.Message);
    }
}
