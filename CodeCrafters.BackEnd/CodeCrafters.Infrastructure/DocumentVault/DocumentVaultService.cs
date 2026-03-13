using CodeCrafters.Application.DocumentVault.Dtos;
using CodeCrafters.Application.DocumentVault.Services;
using CodeCrafters.Domain.Entities;
using CodeCrafters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeCrafters.Infrastructure.DocumentVault;

public class DocumentVaultService(AppDbContext db) : IDocumentVaultService
{
    private const string VaultRoot = "uploads/vault";

    public async Task<List<DocumentVaultItemDto>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await db.DocumentVaultItems
            .Where(d => d.UserId == userId && !d.IsDeleted)
            .OrderBy(d => d.DocumentType)
            .Select(d => new DocumentVaultItemDto
            {
                Id = d.Id,
                DocumentType = d.DocumentType,
                OriginalFileName = d.OriginalFileName,
                ContentType = d.ContentType,
                FileSizeBytes = d.FileSizeBytes,
                UploadedAt = d.UploadedAt
            })
            .ToListAsync(ct);
    }

    public async Task<DocumentVaultItemDto> UploadAsync(Guid userId, string documentType, string fileName, string contentType, Stream fileStream, CancellationToken ct = default)
    {
        // Replace existing document of same type
        var existing = await db.DocumentVaultItems
            .FirstOrDefaultAsync(d => d.UserId == userId && d.DocumentType == documentType && !d.IsDeleted, ct);

        if (existing != null)
        {
            existing.IsDeleted = true;
            existing.UpdatedAt = DateTime.UtcNow;
        }

        // Store file
        var userDir = Path.Combine(VaultRoot, userId.ToString());
        Directory.CreateDirectory(userDir);
        var storedName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        var storedPath = Path.Combine(userDir, storedName);

        await using (var fs = new FileStream(storedPath, FileMode.Create))
        {
            await fileStream.CopyToAsync(fs, ct);
        }

        var item = new DocumentVaultItem
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            DocumentType = documentType,
            OriginalFileName = fileName,
            StoredFilePath = storedPath,
            ContentType = contentType,
            FileSizeBytes = new FileInfo(storedPath).Length,
            UploadedAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        db.DocumentVaultItems.Add(item);
        await db.SaveChangesAsync(ct);

        return new DocumentVaultItemDto
        {
            Id = item.Id,
            DocumentType = item.DocumentType,
            OriginalFileName = item.OriginalFileName,
            ContentType = item.ContentType,
            FileSizeBytes = item.FileSizeBytes,
            UploadedAt = item.UploadedAt
        };
    }

    public async Task DeleteAsync(Guid userId, Guid documentId, CancellationToken ct = default)
    {
        var item = await db.DocumentVaultItems
            .FirstOrDefaultAsync(d => d.Id == documentId && d.UserId == userId && !d.IsDeleted, ct);

        if (item == null)
            throw new KeyNotFoundException("Document not found.");

        item.IsDeleted = true;
        item.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
    }

    public async Task<(Stream Stream, string FileName, string ContentType)?> DownloadAsync(Guid userId, Guid documentId, CancellationToken ct = default)
    {
        var item = await db.DocumentVaultItems
            .FirstOrDefaultAsync(d => d.Id == documentId && d.UserId == userId && !d.IsDeleted, ct);

        if (item == null)
            return null;

        if (!File.Exists(item.StoredFilePath))
            return null;

        var stream = new FileStream(item.StoredFilePath, FileMode.Open, FileAccess.Read);
        return (stream, item.OriginalFileName, item.ContentType);
    }
}
