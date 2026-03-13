using CodeCrafters.Application.Applications.Dtos;

namespace CodeCrafters.Application.Applications.Services;

public interface IApplicationService
{
    Task<List<ApplicationResponseDto>> GetMyApplicationsAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<ApplicationResponseDto> SubmitApplicationAsync(Guid userId, CreateApplicationDto dto, CancellationToken cancellationToken = default);
    Task<ApplicationResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
