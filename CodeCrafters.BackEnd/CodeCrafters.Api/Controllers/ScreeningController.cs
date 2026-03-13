using System.Security.Claims;
using CodeCrafters.Application.Screening.Dtos;
using CodeCrafters.Application.Screening.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeCrafters.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ScreeningController(IEligibilityScreeningService screeningService) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User ID claim missing."));

    /// <summary>Run eligibility screening on a submitted application.</summary>
    [HttpPost("run/{applicationId:guid}")]
    public async Task<ActionResult<ScreeningReportDto>> RunScreening(Guid applicationId, CancellationToken ct)
    {
        var report = await screeningService.RunScreeningAsync(applicationId, ct);
        return Ok(report);
    }

    /// <summary>Get screening report for a specific application.</summary>
    [HttpGet("application/{applicationId:guid}")]
    public async Task<ActionResult<ScreeningReportDto>> GetByApplication(Guid applicationId, CancellationToken ct)
    {
        var report = await screeningService.GetByApplicationIdAsync(applicationId, ct);
        if (report == null) return NotFound();
        return Ok(report);
    }

    /// <summary>Get all screening reports (Program Officer queue). Optional filter: ?result=PendingReview</summary>
    [HttpGet]
    public async Task<ActionResult<List<ScreeningReportDto>>> GetAll([FromQuery] string? result, CancellationToken ct)
    {
        var reports = await screeningService.GetAllAsync(result, ct);
        return Ok(reports);
    }

    /// <summary>Emergency sync: Run screening for any application missing a report.</summary>
    [HttpPost("sync-all")]
    public async Task<ActionResult> SyncAll(CancellationToken ct)
    {
        await screeningService.SyncAllPendingAsync(ct);
        return Ok(new { message = "Sync complete." });
    }

    /// <summary>Program Officer takes action on a screening report.</summary>
    [HttpPost("{reportId:guid}/action")]
    public async Task<ActionResult<ScreeningReportDto>> TakeAction(Guid reportId, [FromBody] ScreeningActionDto action, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var report = await screeningService.TakeActionAsync(reportId, CurrentUserId, action, ct);
        return Ok(report);
    }
}
