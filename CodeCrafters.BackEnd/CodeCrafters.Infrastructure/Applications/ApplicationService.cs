using CodeCrafters.Application.Applications.Dtos;
using CodeCrafters.Application.Applications.Services;
using CodeCrafters.Application.Screening.Services;
using CodeCrafters.Domain.Entities;
using CodeCrafters.Domain.Entities.Applications;
using CodeCrafters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CodeCrafters.Infrastructure.Applications;

public class ApplicationService(AppDbContext dbContext, IEligibilityScreeningService screeningService) : IApplicationService
{
    public async Task<List<ApplicationResponseDto>> GetMyApplicationsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Applications
            .Where(a => a.ApplicantId == userId)
            .Include(a => a.GrantType)
            .Select(a => new ApplicationResponseDto
            {
                Id = a.Id,
                ReferenceNumber = a.ReferenceNumber,
                Title = a.Title,
                StatusLabel = a.StatusLabel,
                RequestedAmount = a.RequestedAmount,
                SubmissionDate = a.SubmissionDate,
                GrantTypeName = a.GrantType.Name,
                CurrentStageId = a.CurrentStageId
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<ApplicationResponseDto> SubmitApplicationAsync(Guid userId, CreateApplicationDto dto, CancellationToken cancellationToken = default)
    {
        // Get the first stage for this grant type
        var firstStage = await dbContext.GrantWorkflowStages
            .Where(s => s.GrantTypeId == dto.GrantTypeId)
            .OrderBy(s => s.OrderIdx)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException("No workflow stages defined for this grant type.");

        var application = new CodeCrafters.Domain.Entities.Applications.Application
        {
            Id = Guid.NewGuid(),
            ReferenceNumber = $"APP-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}",
            GrantTypeId = dto.GrantTypeId,
            ApplicantId = userId,
            Title = dto.Title,
            RequestedAmount = dto.RequestedAmount,
            TotalRequestedAmount = dto.RequestedAmount,
            SubmissionDate = DateTimeOffset.UtcNow,
            CurrentStageId = firstStage.Id,
            StageEnteredAt = DateTimeOffset.UtcNow,
            StatusLabel = firstStage.Name,
            
            // Common form fields
            LegalName = dto.LegalName,
            RegistrationNumber = dto.RegistrationNumber,
            OrganisationType = dto.OrganisationType,
            YearOfEstablishment = dto.YearOfEstablishment,
            StateOfRegistration = dto.StateOfRegistration,
            PrimaryContactName = dto.PrimaryContactName,
            PrimaryContactEmail = dto.PrimaryContactEmail,
            PrimaryContactPhone = dto.PrimaryContactPhone,
            AnnualOperatingBudget = dto.AnnualOperatingBudget,
            ProjectTitle = dto.ProjectTitle,
            ProjectStartDate = dto.ProjectStartDate,
            ProjectEndDate = dto.ProjectEndDate,
            
            // Budget items
            PersonnelCosts = dto.PersonnelCosts,
            EquipmentAndMaterials = dto.EquipmentAndMaterials,
            TravelAndLogistics = dto.TravelAndLogistics,
            TrainingAndWorkshops = dto.TrainingAndWorkshops,
            TechnologySoftware = dto.TechnologySoftware,
            ContentDevelopment = dto.ContentDevelopment,
            SaplingsAndSeeds = dto.SaplingsAndSeeds,
            CommunityEngagementWages = dto.CommunityEngagementWages,
            Overheads = dto.Overheads,
            OtherCosts = dto.OtherCosts,
            BudgetJustification = dto.BudgetJustification,
            
            // Signatory
            AuthorisedSignatoryName = dto.AuthorisedSignatoryName,
            Designation = dto.Designation,
            
            // Feature Specifics
            ProblemStatement = dto.ProblemStatement,
            ProposedSolution = dto.ProposedSolution,
            TargetBeneficiariesCount = dto.TargetBeneficiariesCount,
            InnovationType = dto.InnovationType,
            InnovationDescription = dto.InnovationDescription,
            ThematicArea = dto.ThematicArea,
            EnvironmentalProblemDesc = dto.EnvironmentalProblemDesc,
            
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Applications.Add(application);
        
        // Add to history
        dbContext.ApplicationWorkflowHistories.Add(new ApplicationWorkflowHistory
        {
            Id = Guid.NewGuid(),
            ApplicationId = application.Id,
            StageId = firstStage.Id,
            TransitionedAt = DateTimeOffset.UtcNow,
            StatusLabel = firstStage.Name,
            ActionNotes = "Initial Submission",
            CreatedAt = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync(cancellationToken);
        
        // Trigger automated screening
        try {
            await screeningService.RunScreeningAsync(application.Id, cancellationToken);
        } catch (Exception ex) {
            // Log and continue - we don't want to fail the submission if screening fails
            Console.WriteLine($"Screening failed for application {application.Id}: {ex.Message}");
        }

        return new ApplicationResponseDto
        {
            Id = application.Id,
            ReferenceNumber = application.ReferenceNumber,
            Title = application.Title,
            StatusLabel = application.StatusLabel,
            RequestedAmount = application.RequestedAmount,
            SubmissionDate = application.SubmissionDate,
            CurrentStageId = application.CurrentStageId
        };
    }

    public async Task<ApplicationResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var a = await dbContext.Applications
            .Include(a => a.GrantType)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            
        if (a == null) return null;

        return new ApplicationResponseDto
        {
            Id = a.Id,
            ReferenceNumber = a.ReferenceNumber,
            Title = a.Title,
            StatusLabel = a.StatusLabel,
            RequestedAmount = a.RequestedAmount,
            SubmissionDate = a.SubmissionDate,
            GrantTypeName = a.GrantType.Name,
            CurrentStageId = a.CurrentStageId
        };
    }
}
