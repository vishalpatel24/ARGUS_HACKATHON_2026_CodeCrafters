using System.Security.Claims;
using CodeCrafters.Api.Models;
using CodeCrafters.Application.Documents.Dtos;
using CodeCrafters.Application.Documents.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeCrafters.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DocumentsController(IDocumentVaultService documentVaultService) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User ID claim missing."));

    private static readonly string[] AllowedDocumentTypes = { "RegistrationCertificate", "AuditedFinancials", "Tax80G", "Other" };

    [HttpGet("me")]
    [ProducesResponseType(typeof(IReadOnlyList<UserDocumentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<UserDocumentDto>>> GetMyDocuments(CancellationToken cancellationToken)
    {
        var list = await documentVaultService.GetMyDocumentsAsync(CurrentUserId, cancellationToken);
        return Ok(list);
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(10 * 1024 * 1024)]
    [ProducesResponseType(typeof(UserDocumentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDocumentDto>> Upload([FromForm] UploadDocumentForm form, CancellationToken cancellationToken)
    {
        var file = Request.Form.Files["file"];
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file uploaded." });
        var dt = string.IsNullOrWhiteSpace(form?.DocumentType) || !AllowedDocumentTypes.Contains(form.DocumentType, StringComparer.OrdinalIgnoreCase)
            ? "Other"
            : form.DocumentType;
        await using var stream = file.OpenReadStream();
        var result = await documentVaultService.UploadAsync(
            CurrentUserId, dt, file.FileName, file.ContentType, stream, file.Length, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await documentVaultService.DeleteAsync(id, CurrentUserId, cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
