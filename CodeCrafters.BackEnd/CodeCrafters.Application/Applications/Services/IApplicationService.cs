using CodeCrafters.Application.Applications.Dtos;

namespace CodeCrafters.Application.Applications.Services;

public interface IApplicationService
{
    Task<IReadOnlyList<ApplicationListItemDto>> GetMyApplicationsAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<ApplicationListItemDto?> GetByIdAsync(Guid applicationId, Guid userId, CancellationToken cancellationToken = default);

    Task<ApplicationDraftDto?> GetDraftAsync(Guid applicationId, Guid userId, CancellationToken cancellationToken = default);

    Task<ApplicationListItemDto> CreateDraftAsync(Guid userId, CreateApplicationDto dto, CancellationToken cancellationToken = default);

    Task<ApplicationListItemDto?> UpdateDraftAsync(Guid applicationId, Guid userId, UpdateApplicationDto dto, CancellationToken cancellationToken = default);

    Task<ApplicationListItemDto?> SubmitAsync(Guid applicationId, Guid userId, CancellationToken cancellationToken = default);
}
