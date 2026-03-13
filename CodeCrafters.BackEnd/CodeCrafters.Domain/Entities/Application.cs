using CodeCrafters.Domain.Common;
using CodeCrafters.Domain.Entities;

namespace CodeCrafters.Domain.Entities.Applications;

public class Application : AuditableEntity
{
    public string ReferenceNumber { get; set; } = string.Empty;
    public Guid GrantTypeId { get; set; }
    public GrantType GrantType { get; set; } = null!;

    public Guid ApplicantId { get; set; }
    public User Applicant { get; set; } = null!;
    
    public string Title { get; set; } = string.Empty;
    public decimal RequestedAmount { get; set; }
    public DateTimeOffset? SubmissionDate { get; set; }

    // Workflow Tracking
    public Guid CurrentStageId { get; set; }
    public GrantWorkflowStage CurrentStage { get; set; } = null!;
    public DateTimeOffset StageEnteredAt { get; set; }
    public string StatusLabel { get; set; } = string.Empty; 
    // --- Common Form Fields ---
    public string LegalName { get; set; } = string.Empty;
    public string RegistrationNumber { get; set; } = string.Empty;
    public string OrganisationType { get; set; } = string.Empty; // e.g. NGO, Trust, University
    public int YearOfEstablishment { get; set; }
    public string StateOfRegistration { get; set; } = string.Empty; // Or District & State
    
    public string PrimaryContactName { get; set; } = string.Empty;
    public string PrimaryContactEmail { get; set; } = string.Empty;
    public string PrimaryContactPhone { get; set; } = string.Empty;
    public decimal AnnualOperatingBudget { get; set; }

    public string ProjectTitle { get; set; } = string.Empty;
    public decimal TotalRequestedAmount { get; set; }
    public DateTime? ProjectStartDate { get; set; }
    public DateTime? ProjectEndDate { get; set; }

    // --- Common Budget Breakdown ---
    public decimal PersonnelCosts { get; set; }
    public decimal EquipmentAndMaterials { get; set; }
    public decimal TravelAndLogistics { get; set; }
    public decimal TrainingAndWorkshops { get; set; }
    public decimal TechnologySoftware { get; set; } // Specific to EIG, but mapped here
    public decimal ContentDevelopment { get; set; } // Specific to EIG
    public decimal SaplingsAndSeeds { get; set; } // Specific to ECAG
    public decimal CommunityEngagementWages { get; set; } // Specific to ECAG
    public decimal Overheads { get; set; }
    public decimal OtherCosts { get; set; }
    public string? BudgetJustification { get; set; }

    // --- Common Declaration ---
    public string AuthorisedSignatoryName { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;

    // --- CDG Specific Fields ---
    public string? ProblemStatement { get; set; }
    public string? ProposedSolution { get; set; }
    public int? TargetBeneficiariesCount { get; set; }
    public string? BeneficiaryDemographics { get; set; }
    public string? KeyActivitiesList { get; set; }
    public string? ExpectedOutcomes { get; set; }
    public string? RelevantPriorProjectsDesc { get; set; }
    public bool? HasReceivedGrantsBefore { get; set; }
    public string? PriorFunderNameAndAmount { get; set; }

    // --- EIG Specific Fields ---
    public string? InnovationType { get; set; }
    public string? ProblemBeingSolved { get; set; }
    public string? InnovationDescription { get; set; }
    public string? TargetSchoolsDistricts { get; set; }
    public int? NumberOfSchoolsTargeted { get; set; }
    public int? NumberOfStudentsToBenefit { get; set; }
    public string? GradeLevelsTargeted { get; set; }
    public string? TechnologyToolsUsed { get; set; }
    public string? EvidenceBase { get; set; }
    public string? ProjectLeadNameAndQuals { get; set; }
    public int? TeamSize { get; set; }
    public string? TeamExpertiseDescription { get; set; }
    public string? KeyPartners { get; set; }
    public string? PrimaryLearningOutcome { get; set; }
    public string? MeasurementPlan { get; set; }
    public string? BaselineAssessmentPlan { get; set; }
    public string? KeyMilestones { get; set; } // list 3-6

    // --- ECAG Specific Fields ---
    public string? ThematicArea { get; set; }
    public string? EnvironmentalProblemDesc { get; set; }
    public string? ProposedIntervention { get; set; }
    public string? GeographicCoverage { get; set; } // hectares / km coastline
    public int? DirectBeneficiariesCount { get; set; }
    public string? CommunityInvolvementPlan { get; set; }
    public string? EnvironmentalIndicators { get; set; } // list 3-5
    public string? ClimateVulnerabilityContext { get; set; }
    public string? RiskOfNotActing { get; set; }
    public string? ProjectLeadNameAndExpertise { get; set; }
    public string? TeamDescription { get; set; }
    public string? GovernmentPartnerships { get; set; }
    public string? ActivityPlanForFirst6Months { get; set; }

    public ICollection<ApplicationWorkflowHistory> WorkflowHistories { get; set; } = new List<ApplicationWorkflowHistory>();
}
