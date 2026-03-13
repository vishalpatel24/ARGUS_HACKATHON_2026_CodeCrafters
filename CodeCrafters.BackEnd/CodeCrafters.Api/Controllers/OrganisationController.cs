using System.Security.Claims;
using CodeCrafters.Application.Organisations.Dtos;
using CodeCrafters.Application.Organisations.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeCrafters.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrganisationController(IOrganisationService organisationService) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User ID claim missing."));

    /// <summary>Get the current user's organisation profile (null 404 if not yet created).</summary>
    [HttpGet("me")]
    [ProducesResponseType(typeof(OrganisationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrganisationDto>> GetMyOrganisation(CancellationToken cancellationToken)
    {
        var org = await organisationService.GetByUserIdAsync(CurrentUserId, cancellationToken);
        return org is null ? NotFound() : Ok(org);
    }

    /// <summary>Create or update the current user's organisation profile.</summary>
    [HttpPut("me")]
    [ProducesResponseType(typeof(OrganisationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrganisationDto>> UpsertMyOrganisation(
        [FromBody] UpsertOrganisationDto dto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await organisationService.UpsertAsync(CurrentUserId, dto, cancellationToken);
        return Ok(result);
    }
}
