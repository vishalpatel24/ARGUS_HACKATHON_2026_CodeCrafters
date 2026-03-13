namespace CodeCrafters.Application.Documents.Dtos;

public sealed class UserDocumentDto
{
    public Guid Id { get; init; }
    public string DocumentType { get; init; } = string.Empty;
    public string FileName { get; init; } = string.Empty;
    public string ContentType { get; init; } = string.Empty;
    public long FileSizeBytes { get; init; }
    public DateTime UploadedAt { get; init; }
}
