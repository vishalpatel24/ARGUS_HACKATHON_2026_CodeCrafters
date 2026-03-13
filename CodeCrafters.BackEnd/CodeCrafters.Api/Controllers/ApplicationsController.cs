using System.Security.Claims;
using CodeCrafters.Application.Applications.Dtos;
using CodeCrafters.Application.Applications.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeCrafters.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApplicationsController(IApplicationService applicationService) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User ID claim missing."));

    [HttpGet("my")]
    public async Task<ActionResult<List<ApplicationResponseDto>>> GetMyApplications(CancellationToken cancellationToken)
    {
        return Ok(await applicationService.GetMyApplicationsAsync(CurrentUserId, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<ApplicationResponseDto>> SubmitApplication(
        [FromBody] CreateApplicationDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await applicationService.SubmitApplicationAsync(CurrentUserId, dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApplicationResponseDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var app = await applicationService.GetByIdAsync(id, cancellationToken);
        return app is null ? NotFound() : Ok(app);
    }
}
