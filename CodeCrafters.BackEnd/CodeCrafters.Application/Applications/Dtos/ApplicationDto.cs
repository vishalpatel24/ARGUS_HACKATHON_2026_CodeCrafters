using System.ComponentModel.DataAnnotations;

namespace CodeCrafters.Application.Applications.Dtos;

public class CreateApplicationDto
{
    [Required]
    public Guid GrantTypeId { get; init; }

    [Required]
    [StringLength(200)]
    public string Title { get; init; } = string.Empty;

    [Required]
    public decimal RequestedAmount { get; init; }

    // Common fields
    [Required]
    public string LegalName { get; init; } = string.Empty;
    [Required]
    public string RegistrationNumber { get; init; } = string.Empty;
    [Required]
    public string OrganisationType { get; init; } = string.Empty;
    [Required]
    public int YearOfEstablishment { get; init; }
    [Required]
    public string StateOfRegistration { get; init; } = string.Empty;
    
    [Required]
    public string PrimaryContactName { get; init; } = string.Empty;
    [Required]
    [EmailAddress]
    public string PrimaryContactEmail { get; init; } = string.Empty;
    [Required]
    public string PrimaryContactPhone { get; init; } = string.Empty;
    
    [Required]
    public decimal AnnualOperatingBudget { get; init; }

    [Required]
    public string ProjectTitle { get; init; } = string.Empty;
    
    public DateTime? ProjectStartDate { get; init; }
    public DateTime? ProjectEndDate { get; init; }

    // Budget
    public decimal PersonnelCosts { get; init; }
    public decimal EquipmentAndMaterials { get; init; }
    public decimal TravelAndLogistics { get; init; }
    public decimal TrainingAndWorkshops { get; init; }
    public decimal TechnologySoftware { get; init; }
    public decimal ContentDevelopment { get; init; }
    public decimal SaplingsAndSeeds { get; init; }
    public decimal CommunityEngagementWages { get; init; }
    public decimal Overheads { get; init; }
    public decimal OtherCosts { get; init; }
    public string? BudgetJustification { get; init; }

    // Declaration
    [Required]
    public string AuthorisedSignatoryName { get; init; } = string.Empty;
    [Required]
    public string Designation { get; init; } = string.Empty;

    // CDG specific
    public string? ProblemStatement { get; init; }
    public string? ProposedSolution { get; init; }
    public int? TargetBeneficiariesCount { get; init; }

    // EIG specific
    public string? InnovationType { get; init; }
    public string? InnovationDescription { get; init; }

    // ECAG specific
    public string? ThematicArea { get; init; }
    public string? EnvironmentalProblemDesc { get; init; }
}

public class ApplicationResponseDto
{
    public Guid Id { get; init; }
    public string ReferenceNumber { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string StatusLabel { get; init; } = string.Empty;
    public decimal RequestedAmount { get; init; }
    public DateTimeOffset? SubmissionDate { get; init; }
    public string GrantTypeName { get; init; } = string.Empty;
    public Guid CurrentStageId { get; init; }
}
