namespace CodeCrafters.Domain.Entities;

/// <summary>
/// Document uploaded by an applicant to their document vault (e.g. Registration Certificate, Audited Financials, 80G).
/// </summary>
public class UserDocument
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string DocumentType { get; set; } = string.Empty; // RegistrationCertificate, AuditedFinancials, Tax80G, Other
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string StoredPath { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }
    public DateTime UploadedAt { get; set; }
}
