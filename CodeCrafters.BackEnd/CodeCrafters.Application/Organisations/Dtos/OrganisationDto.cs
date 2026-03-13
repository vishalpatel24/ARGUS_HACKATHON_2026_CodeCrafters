namespace CodeCrafters.Application.Organisations.Dtos;

public sealed class OrganisationDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string RegistrationNumber { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public decimal AnnualBudget { get; init; }
    public string ContactPersonName { get; init; } = string.Empty;
    public string ContactPersonEmail { get; init; } = string.Empty;
    public string ContactPersonPhone { get; init; } = string.Empty;
    public bool IsProfileComplete { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}
