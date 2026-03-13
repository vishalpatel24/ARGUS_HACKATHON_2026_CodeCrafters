using CodeCrafters.Domain.Common;

namespace CodeCrafters.Domain.Entities;

public class DocumentVaultItem : AuditableEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    /// <summary>Category: RegistrationCertificate, AuditedFinancials, 80GCertificate, FCRACertificate, Other</summary>
    public string DocumentType { get; set; } = string.Empty;

    public string OriginalFileName { get; set; } = string.Empty;

    /// <summary>Relative path on disk where the file is stored.</summary>
    public string StoredFilePath { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long FileSizeBytes { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
