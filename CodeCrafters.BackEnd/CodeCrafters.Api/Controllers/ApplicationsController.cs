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

    /// <summary>List all applications for the current user (applicant).</summary>
    [HttpGet("me")]
    [ProducesResponseType(typeof(IReadOnlyList<ApplicationListItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ApplicationListItemDto>>> GetMyApplications(CancellationToken cancellationToken)
    {
        var list = await applicationService.GetMyApplicationsAsync(CurrentUserId, cancellationToken);
        return Ok(list);
    }

    /// <summary>Get a single application by id (must belong to current user).</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApplicationListItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApplicationListItemDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var app = await applicationService.GetByIdAsync(id, CurrentUserId, cancellationToken);
        return app is null ? NotFound() : Ok(app);
    }

    /// <summary>Get draft application details for editing (wizard).</summary>
    [HttpGet("{id:guid}/draft")]
    [ProducesResponseType(typeof(ApplicationDraftDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApplicationDraftDto>> GetDraft(Guid id, CancellationToken cancellationToken)
    {
        var draft = await applicationService.GetDraftAsync(id, CurrentUserId, cancellationToken);
        return draft is null ? NotFound() : Ok(draft);
    }

    /// <summary>Create a new draft application for a grant type.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApplicationListItemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApplicationListItemDto>> Create([FromBody] CreateApplicationDto dto, CancellationToken cancellationToken)
    {
        if (dto.GrantTypeId == Guid.Empty)
            return BadRequest(new { message = "GrantTypeId is required." });

        try
        {
            var app = await applicationService.CreateDraftAsync(CurrentUserId, dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = app.Id }, app);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Update a draft application (partial update for wizard steps).</summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApplicationListItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApplicationListItemDto>> Update(Guid id, [FromBody] UpdateApplicationDto dto, CancellationToken cancellationToken)
    {
        var app = await applicationService.UpdateDraftAsync(id, CurrentUserId, dto, cancellationToken);
        return app is null ? NotFound() : Ok(app);
    }

    /// <summary>Submit a draft application.</summary>
    [HttpPost("{id:guid}/submit")]
    [ProducesResponseType(typeof(ApplicationListItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApplicationListItemDto>> Submit(Guid id, CancellationToken cancellationToken)
    {
        var app = await applicationService.SubmitAsync(id, CurrentUserId, cancellationToken);
        return app is null ? NotFound() : Ok(app);
    }
}
