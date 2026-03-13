using CodeCrafters.Application.Organisations.Dtos;
using CodeCrafters.Application.Organisations.Services;
using CodeCrafters.Domain.Entities.Organisations;
using CodeCrafters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeCrafters.Infrastructure.Organisations;

public sealed class OrganisationService(AppDbContext dbContext) : IOrganisationService
{
    public async Task<OrganisationDto?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var org = await dbContext.Organisations
            .FirstOrDefaultAsync(o => o.UserId == userId, cancellationToken);

        return org is null ? null : MapToDto(org);
    }

    public async Task<OrganisationDto> UpsertAsync(Guid userId, UpsertOrganisationDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await dbContext.Organisations
            .FirstOrDefaultAsync(o => o.UserId == userId, cancellationToken);

        if (existing is null)
        {
            existing = new Organisation
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            dbContext.Organisations.Add(existing);
        }

        existing.Name = dto.Name;
        existing.RegistrationNumber = dto.RegistrationNumber;
        existing.Type = dto.Type;
        existing.State = dto.State;
        existing.AnnualBudget = dto.AnnualBudget;
        existing.ContactPersonName = dto.ContactPersonName;
        existing.ContactPersonEmail = dto.ContactPersonEmail;
        existing.ContactPersonPhone = dto.ContactPersonPhone;
        existing.IsProfileComplete = true;
        existing.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        return MapToDto(existing);
    }

    private static OrganisationDto MapToDto(Organisation org) => new()
    {
        Id = org.Id,
        UserId = org.UserId,
        Name = org.Name,
        RegistrationNumber = org.RegistrationNumber,
        Type = org.Type,
        State = org.State,
        AnnualBudget = org.AnnualBudget,
        ContactPersonName = org.ContactPersonName,
        ContactPersonEmail = org.ContactPersonEmail,
        ContactPersonPhone = org.ContactPersonPhone,
        IsProfileComplete = org.IsProfileComplete,
        CreatedAt = org.CreatedAt,
        UpdatedAt = org.UpdatedAt
    };
}
