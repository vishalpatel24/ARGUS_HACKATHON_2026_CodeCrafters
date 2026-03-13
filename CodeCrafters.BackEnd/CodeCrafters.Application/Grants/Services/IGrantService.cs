using CodeCrafters.Application.Grants.Dtos;

namespace CodeCrafters.Application.Grants.Services;

public interface IGrantService
{
    Task<List<GrantTypeDto>> GetAllGrantsAsync(CancellationToken cancellationToken = default);
    Task<GrantTypeDto?> GetGrantByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
