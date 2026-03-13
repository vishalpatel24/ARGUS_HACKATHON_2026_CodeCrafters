namespace CodeCrafters.Api.Models;

/// <summary>
/// Form model for document vault upload. File is read from Request.Form.Files in the controller so Swagger can document the operation.
/// </summary>
public class UploadDocumentForm
{
    public string DocumentType { get; set; } = "Other";
    // File is not a property so Swagger does not reflect IFormFile; controller reads file from HttpContext.Request.Form.Files
}
