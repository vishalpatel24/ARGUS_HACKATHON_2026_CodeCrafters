using System.Security.Claims;
using CodeCrafters.Application.DocumentVault.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeCrafters.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DocumentVaultController(IDocumentVaultService vaultService) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User ID claim missing."));

    /// <summary>List all vault documents for the current user.</summary>
    [HttpGet]
    public async Task<IActionResult> GetMyDocuments(CancellationToken ct)
    {
        var docs = await vaultService.GetByUserIdAsync(CurrentUserId, ct);
        return Ok(docs);
    }

    /// <summary>Upload a document to the vault. documentType: RegistrationCertificate, AuditedFinancials, 80GCertificate, FCRACertificate, Other</summary>
    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload([FromForm] string documentType, IFormFile file, CancellationToken ct)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file provided.");

        var validTypes = new[] { "RegistrationCertificate", "AuditedFinancials", "80GCertificate", "FCRACertificate", "Other" };
        if (!validTypes.Contains(documentType))
            return BadRequest($"Invalid document type. Valid types: {string.Join(", ", validTypes)}");

        await using var stream = file.OpenReadStream();
        var result = await vaultService.UploadAsync(CurrentUserId, documentType, file.FileName, file.ContentType, stream, ct);
        return Ok(result);
    }

    /// <summary>Delete a vault document.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await vaultService.DeleteAsync(CurrentUserId, id, ct);
        return NoContent();
    }

    /// <summary>Download a vault document.</summary>
    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> Download(Guid id, CancellationToken ct)
    {
        var result = await vaultService.DownloadAsync(CurrentUserId, id, ct);
        if (result == null)
            return NotFound();

        return File(result.Value.Stream, result.Value.ContentType, result.Value.FileName);
    }
}
