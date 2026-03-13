using CodeCrafters.Application.DocumentVault.Dtos;

namespace CodeCrafters.Application.DocumentVault.Services;

public interface IDocumentVaultService
{
    Task<List<DocumentVaultItemDto>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<DocumentVaultItemDto> UploadAsync(Guid userId, string documentType, string fileName, string contentType, Stream fileStream, CancellationToken ct = default);
    Task DeleteAsync(Guid userId, Guid documentId, CancellationToken ct = default);
    Task<(Stream Stream, string FileName, string ContentType)?> DownloadAsync(Guid userId, Guid documentId, CancellationToken ct = default);
}
