using CodeCrafters.Application.Grants.Dtos;
using CodeCrafters.Application.Grants.Services;
using CodeCrafters.Domain.Entities;
using CodeCrafters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeCrafters.Infrastructure.Grants;

public sealed class GrantService(AppDbContext dbContext) : IGrantService
{
    public async Task<List<GrantTypeDto>> GetAllGrantsAsync(CancellationToken cancellationToken = default)
    {
        var grants = await dbContext.Set<GrantType>()
            .OrderBy(g => g.Name)
            .ToListAsync(cancellationToken);

        return grants.Select(MapToDto).ToList();
    }

    public async Task<GrantTypeDto?> GetGrantByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var grant = await dbContext.Set<GrantType>()
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
            
        return grant is null ? null : MapToDto(grant);
    }

    private static GrantTypeDto MapToDto(GrantType grant) => new()
    {
        Id = grant.Id,
        Code = grant.Code,
        Name = grant.Name,
        Purpose = grant.Purpose,
        FundingMinAmount = grant.FundingMinAmount,
        FundingMaxAmount = grant.FundingMaxAmount,
        ProjectDurationMinMonths = grant.ProjectDurationMinMonths,
        ProjectDurationMaxMonths = grant.ProjectDurationMaxMonths,
        EligibleApplicants = grant.EligibleApplicants,
        GeographicFocus = grant.GeographicFocus,
        AnnualCycle = grant.AnnualCycle,
        MaximumAwardsPerCycle = grant.MaximumAwardsPerCycle,
        TotalProgrammeBudget = grant.TotalProgrammeBudget
    };
}
