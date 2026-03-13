using CodeCrafters.Application.Applications.Dtos;
using CodeCrafters.Application.Applications.Services;
using CodeCrafters.Domain.Entities.Applications;
using CodeCrafters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeCrafters.Infrastructure.Applications;

public sealed class ApplicationService(AppDbContext db) : IApplicationService
{
    public async Task<IReadOnlyList<ApplicationListItemDto>> GetMyApplicationsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var list = await db.Applications
            .AsNoTracking()
            .Where(a => a.ApplicantId == userId)
            .OrderByDescending(a => a.StageEnteredAt)
            .Select(a => new ApplicationListItemDto
            {
                Id = a.Id,
                ReferenceNumber = a.ReferenceNumber,
                Title = a.Title,
                GrantTypeName = a.GrantType.Name,
                StatusLabel = a.StatusLabel,
                CurrentStageName = a.CurrentStage.Name,
                LastUpdated = a.StageEnteredAt,
                SubmissionDate = a.SubmissionDate
            })
            .ToListAsync(cancellationToken);

        return list;
    }

    public async Task<ApplicationListItemDto?> GetByIdAsync(Guid applicationId, Guid userId, CancellationToken cancellationToken = default)
    {
        var app = await db.Applications
            .AsNoTracking()
            .Where(a => a.Id == applicationId && a.ApplicantId == userId)
            .Select(a => new ApplicationListItemDto
            {
                Id = a.Id,
                ReferenceNumber = a.ReferenceNumber,
                Title = a.Title,
                GrantTypeName = a.GrantType.Name,
                StatusLabel = a.StatusLabel,
                CurrentStageName = a.CurrentStage.Name,
                LastUpdated = a.StageEnteredAt,
                SubmissionDate = a.SubmissionDate
            })
            .FirstOrDefaultAsync(cancellationToken);

        return app;
    }

    public async Task<ApplicationDraftDto?> GetDraftAsync(Guid applicationId, Guid userId, CancellationToken cancellationToken = default)
    {
        var app = await db.Applications
            .AsNoTracking()
            .Where(a => a.Id == applicationId && a.ApplicantId == userId && a.StatusLabel == "Draft")
            .Select(a => new ApplicationDraftDto
            {
                Id = a.Id,
                ReferenceNumber = a.ReferenceNumber,
                GrantTypeId = a.GrantTypeId,
                GrantTypeName = a.GrantType.Name,
                Title = a.Title,
                StatusLabel = a.StatusLabel,
                LegalName = a.LegalName,
                RegistrationNumber = a.RegistrationNumber,
                OrganisationType = a.OrganisationType,
                YearOfEstablishment = a.YearOfEstablishment,
                StateOfRegistration = a.StateOfRegistration,
                PrimaryContactName = a.PrimaryContactName,
                PrimaryContactEmail = a.PrimaryContactEmail,
                PrimaryContactPhone = a.PrimaryContactPhone,
                AnnualOperatingBudget = a.AnnualOperatingBudget,
                ProjectTitle = a.ProjectTitle,
                TotalRequestedAmount = a.TotalRequestedAmount,
                ProjectStartDate = a.ProjectStartDate,
                ProjectEndDate = a.ProjectEndDate,
                PersonnelCosts = a.PersonnelCosts,
                EquipmentAndMaterials = a.EquipmentAndMaterials,
                TravelAndLogistics = a.TravelAndLogistics,
                Overheads = a.Overheads,
                OtherCosts = a.OtherCosts,
                BudgetJustification = a.BudgetJustification,
                AuthorisedSignatoryName = a.AuthorisedSignatoryName,
                Designation = a.Designation,
                ProblemStatement = a.ProblemStatement,
                ProposedSolution = a.ProposedSolution,
                TargetBeneficiariesCount = a.TargetBeneficiariesCount,
                ExpectedOutcomes = a.ExpectedOutcomes
            })
            .FirstOrDefaultAsync(cancellationToken);

        return app;
    }

    public async Task<ApplicationListItemDto> CreateDraftAsync(Guid userId, CreateApplicationDto dto, CancellationToken cancellationToken = default)
    {
        var firstStage = await db.GrantWorkflowStages
            .Where(s => s.GrantTypeId == dto.GrantTypeId)
            .OrderBy(s => s.OrderIdx)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException("Grant type has no workflow stages configured.");

        var grantType = await db.GrantTypes.FindAsync([dto.GrantTypeId], cancellationToken)
            ?? throw new ArgumentException("Grant type not found.", nameof(dto.GrantTypeId));

        var refNumber = $"APP-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";

        var app = new CodeCrafters.Domain.Entities.Applications.Application
        {
            Id = Guid.NewGuid(),
            ReferenceNumber = refNumber,
            GrantTypeId = dto.GrantTypeId,
            ApplicantId = userId,
            Title = dto.Title,
            RequestedAmount = 0,
            CurrentStageId = firstStage.Id,
            StageEnteredAt = DateTimeOffset.UtcNow,
            StatusLabel = "Draft",
            CreatedAt = DateTime.UtcNow
        };

        db.Applications.Add(app);
        await db.SaveChangesAsync(cancellationToken);

        return new ApplicationListItemDto
        {
            Id = app.Id,
            ReferenceNumber = app.ReferenceNumber,
            Title = app.Title,
            GrantTypeName = grantType.Name,
            StatusLabel = app.StatusLabel,
            CurrentStageName = firstStage.Name,
            LastUpdated = app.StageEnteredAt,
            SubmissionDate = null
        };
    }

    public async Task<ApplicationListItemDto?> UpdateDraftAsync(Guid applicationId, Guid userId, UpdateApplicationDto dto, CancellationToken cancellationToken = default)
    {
        var app = await db.Applications
            .FirstOrDefaultAsync(a => a.Id == applicationId && a.ApplicantId == userId && a.StatusLabel == "Draft", cancellationToken);

        if (app is null) return null;

        if (dto.Title is not null) app.Title = dto.Title;
        if (dto.LegalName is not null) app.LegalName = dto.LegalName;
        if (dto.RegistrationNumber is not null) app.RegistrationNumber = dto.RegistrationNumber;
        if (dto.OrganisationType is not null) app.OrganisationType = dto.OrganisationType;
        if (dto.YearOfEstablishment.HasValue) app.YearOfEstablishment = dto.YearOfEstablishment.Value;
        if (dto.StateOfRegistration is not null) app.StateOfRegistration = dto.StateOfRegistration;
        if (dto.PrimaryContactName is not null) app.PrimaryContactName = dto.PrimaryContactName;
        if (dto.PrimaryContactEmail is not null) app.PrimaryContactEmail = dto.PrimaryContactEmail;
        if (dto.PrimaryContactPhone is not null) app.PrimaryContactPhone = dto.PrimaryContactPhone;
        if (dto.AnnualOperatingBudget.HasValue) app.AnnualOperatingBudget = dto.AnnualOperatingBudget.Value;
        if (dto.ProjectTitle is not null) app.ProjectTitle = dto.ProjectTitle;
        if (dto.TotalRequestedAmount.HasValue) app.TotalRequestedAmount = dto.TotalRequestedAmount.Value;
        if (dto.ProjectStartDate.HasValue) app.ProjectStartDate = dto.ProjectStartDate;
        if (dto.ProjectEndDate.HasValue) app.ProjectEndDate = dto.ProjectEndDate;
        if (dto.PersonnelCosts.HasValue) app.PersonnelCosts = dto.PersonnelCosts.Value;
        if (dto.EquipmentAndMaterials.HasValue) app.EquipmentAndMaterials = dto.EquipmentAndMaterials.Value;
        if (dto.TravelAndLogistics.HasValue) app.TravelAndLogistics = dto.TravelAndLogistics.Value;
        if (dto.Overheads.HasValue) app.Overheads = dto.Overheads.Value;
        if (dto.OtherCosts.HasValue) app.OtherCosts = dto.OtherCosts.Value;
        if (dto.BudgetJustification is not null) app.BudgetJustification = dto.BudgetJustification;
        if (dto.AuthorisedSignatoryName is not null) app.AuthorisedSignatoryName = dto.AuthorisedSignatoryName;
        if (dto.Designation is not null) app.Designation = dto.Designation;
        if (dto.ProblemStatement is not null) app.ProblemStatement = dto.ProblemStatement;
        if (dto.ProposedSolution is not null) app.ProposedSolution = dto.ProposedSolution;
        if (dto.TargetBeneficiariesCount.HasValue) app.TargetBeneficiariesCount = dto.TargetBeneficiariesCount;
        if (dto.ExpectedOutcomes is not null) app.ExpectedOutcomes = dto.ExpectedOutcomes;

        app.RequestedAmount = app.TotalRequestedAmount;
        app.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(cancellationToken);

        return await GetByIdAsync(applicationId, userId, cancellationToken);
    }

    public async Task<ApplicationListItemDto?> SubmitAsync(Guid applicationId, Guid userId, CancellationToken cancellationToken = default)
    {
        var app = await db.Applications
            .Include(a => a.GrantType)
            .Include(a => a.CurrentStage)
            .FirstOrDefaultAsync(a => a.Id == applicationId && a.ApplicantId == userId && a.StatusLabel == "Draft", cancellationToken);

        if (app is null) return null;

        app.StatusLabel = "Submitted";
        app.SubmissionDate = DateTimeOffset.UtcNow;
        app.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(cancellationToken);

        return new ApplicationListItemDto
        {
            Id = app.Id,
            ReferenceNumber = app.ReferenceNumber,
            Title = app.Title,
            GrantTypeName = app.GrantType.Name,
            StatusLabel = app.StatusLabel,
            CurrentStageName = app.CurrentStage.Name,
            LastUpdated = app.StageEnteredAt,
            SubmissionDate = app.SubmissionDate
        };
    }
}
