namespace CodeCrafters.Application.Grants.Dtos;

public class GrantTypeDto
{
    public Guid Id { get; init; }
    public string Code { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Purpose { get; init; } = string.Empty;
    public decimal FundingMinAmount { get; init; }
    public decimal FundingMaxAmount { get; init; }
    public int ProjectDurationMinMonths { get; init; }
    public int ProjectDurationMaxMonths { get; init; }
    public string EligibleApplicants { get; init; } = string.Empty;
    public string GeographicFocus { get; init; } = string.Empty;
    public string AnnualCycle { get; init; } = string.Empty;
    public int MaximumAwardsPerCycle { get; init; }
    public decimal TotalProgrammeBudget { get; init; }
}
