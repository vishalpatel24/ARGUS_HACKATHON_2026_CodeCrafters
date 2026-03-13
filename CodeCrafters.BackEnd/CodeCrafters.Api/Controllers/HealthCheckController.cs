using CodeCrafters.Application.HealthCheck.Dtos;
using CodeCrafters.Application.HealthCheck.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeCrafters.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthCheckController(IHealthCheckService healthCheckService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(HealthCheckResponseDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<HealthCheckResponseDto>> Get(CancellationToken cancellationToken)
    {
        var result = await healthCheckService.CheckHealthAsync(cancellationToken);
        return Ok(result);
    }
}

