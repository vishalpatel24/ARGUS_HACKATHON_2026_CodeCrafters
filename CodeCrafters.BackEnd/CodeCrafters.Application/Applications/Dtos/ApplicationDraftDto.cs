namespace CodeCrafters.Application.Applications.Dtos;

public sealed class ApplicationDraftDto
{
    public Guid Id { get; init; }
    public string ReferenceNumber { get; init; } = string.Empty;
    public Guid GrantTypeId { get; init; }
    public string GrantTypeName { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string StatusLabel { get; init; } = string.Empty;
    public string? LegalName { get; init; }
    public string? RegistrationNumber { get; init; }
    public string? OrganisationType { get; init; }
    public int YearOfEstablishment { get; init; }
    public string? StateOfRegistration { get; init; }
    public string? PrimaryContactName { get; init; }
    public string? PrimaryContactEmail { get; init; }
    public string? PrimaryContactPhone { get; init; }
    public decimal AnnualOperatingBudget { get; init; }
    public string? ProjectTitle { get; init; }
    public decimal TotalRequestedAmount { get; init; }
    public DateTime? ProjectStartDate { get; init; }
    public DateTime? ProjectEndDate { get; init; }
    public decimal PersonnelCosts { get; init; }
    public decimal EquipmentAndMaterials { get; init; }
    public decimal TravelAndLogistics { get; init; }
    public decimal Overheads { get; init; }
    public decimal OtherCosts { get; init; }
    public string? BudgetJustification { get; init; }
    public string? AuthorisedSignatoryName { get; init; }
    public string? Designation { get; init; }
    public string? ProblemStatement { get; init; }
    public string? ProposedSolution { get; init; }
    public int? TargetBeneficiariesCount { get; init; }
    public string? ExpectedOutcomes { get; init; }
}
