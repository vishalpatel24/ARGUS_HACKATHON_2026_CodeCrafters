using CodeCrafters.Application.Organisations.Dtos;

namespace CodeCrafters.Application.Organisations.Services;

public interface IOrganisationService
{
    /// <summary>Get the organisation profile for the given user, or null if none exists.</summary>
    Task<OrganisationDto?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    /// <summary>Create or update the organisation profile for the given user.</summary>
    Task<OrganisationDto> UpsertAsync(Guid userId, UpsertOrganisationDto dto, CancellationToken cancellationToken = default);
}
