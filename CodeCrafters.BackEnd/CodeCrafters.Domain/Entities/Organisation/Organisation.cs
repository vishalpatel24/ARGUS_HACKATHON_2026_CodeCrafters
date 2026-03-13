using CodeCrafters.Domain.Common;

namespace CodeCrafters.Domain.Entities.Organisation;

public class Organisation : AuditableEntity
{
    public string Name { get; set; } = string.Empty;

    public OrganisationType OrganisationType { get; set; }

    public string? RegistrationNumber { get; set; }

    public int? YearEstablished { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? PostalCode { get; set; }

    public string? Website { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public decimal? AnnualBudget { get; set; }

    public int? NumberOfEmployees { get; set; }

    public string? PrimaryContactName { get; set; }

    public string? PrimaryContactEmail { get; set; }

    public string? PrimaryContactPhone { get; set; }

    public string? Description { get; set; }
}

