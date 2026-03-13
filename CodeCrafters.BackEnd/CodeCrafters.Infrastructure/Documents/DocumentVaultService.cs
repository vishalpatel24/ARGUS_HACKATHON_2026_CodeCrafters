using CodeCrafters.Application.Documents.Dtos;
using CodeCrafters.Application.Documents.Services;
using CodeCrafters.Domain.Entities;
using CodeCrafters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace CodeCrafters.Infrastructure.Documents;

public sealed class DocumentVaultService(AppDbContext db, IHostEnvironment env) : IDocumentVaultService
{
    private const string UploadSubDir = "uploads/documents";
    private static readonly string[] AllowedTypes = { "RegistrationCertificate", "AuditedFinancials", "Tax80G", "Other" };

    public async Task<IReadOnlyList<UserDocumentDto>> GetMyDocumentsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await db.UserDocuments
            .AsNoTracking()
            .Where(d => d.UserId == userId)
            .OrderByDescending(d => d.UploadedAt)
            .Select(d => new UserDocumentDto
            {
                Id = d.Id,
                DocumentType = d.DocumentType,
                FileName = d.FileName,
                ContentType = d.ContentType,
                FileSizeBytes = d.FileSizeBytes,
                UploadedAt = d.UploadedAt
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<UserDocumentDto?> UploadAsync(Guid userId, string documentType, string fileName, string contentType, Stream fileStream, long fileSizeBytes, CancellationToken cancellationToken = default)
    {
        if (!AllowedTypes.Contains(documentType, StringComparer.OrdinalIgnoreCase))
            documentType = "Other";

        var safeFileName = Path.GetFileName(fileName);
        if (string.IsNullOrWhiteSpace(safeFileName))
            safeFileName = "document";
        var ext = Path.GetExtension(safeFileName);
        var storedFileName = $"{Guid.NewGuid():N}{ext}";
        var userDir = Path.Combine(env.ContentRootPath, UploadSubDir, userId.ToString("N"));
        Directory.CreateDirectory(userDir);
        var fullPath = Path.Combine(userDir, storedFileName);
        await using (var fs = File.Create(fullPath))
            await fileStream.CopyToAsync(fs, cancellationToken);

        var relativePath = Path.Combine(UploadSubDir, userId.ToString("N"), storedFileName).Replace('\\', '/');
        var doc = new UserDocument
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            DocumentType = documentType,
            FileName = safeFileName,
            ContentType = contentType,
            StoredPath = relativePath,
            FileSizeBytes = fileSizeBytes,
            UploadedAt = DateTime.UtcNow
        };
        db.UserDocuments.Add(doc);
        await db.SaveChangesAsync(cancellationToken);

        return new UserDocumentDto
        {
            Id = doc.Id,
            DocumentType = doc.DocumentType,
            FileName = doc.FileName,
            ContentType = doc.ContentType,
            FileSizeBytes = doc.FileSizeBytes,
            UploadedAt = doc.UploadedAt
        };
    }

    public async Task<bool> DeleteAsync(Guid documentId, Guid userId, CancellationToken cancellationToken = default)
    {
        var doc = await db.UserDocuments.FirstOrDefaultAsync(d => d.Id == documentId && d.UserId == userId, cancellationToken);
        if (doc is null) return false;
        var fullPath = Path.Combine(env.ContentRootPath, doc.StoredPath.Replace('/', Path.DirectorySeparatorChar));
        if (File.Exists(fullPath))
            File.Delete(fullPath);
        db.UserDocuments.Remove(doc);
        await db.SaveChangesAsync(cancellationToken);
        return true;
    }
}
