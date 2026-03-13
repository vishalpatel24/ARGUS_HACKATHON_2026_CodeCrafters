using CodeCrafters.Application.Documents.Dtos;

namespace CodeCrafters.Application.Documents.Services;

public interface IDocumentVaultService
{
    Task<IReadOnlyList<UserDocumentDto>> GetMyDocumentsAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<UserDocumentDto?> UploadAsync(Guid userId, string documentType, string fileName, string contentType, Stream fileStream, long fileSizeBytes, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(Guid documentId, Guid userId, CancellationToken cancellationToken = default);
}
