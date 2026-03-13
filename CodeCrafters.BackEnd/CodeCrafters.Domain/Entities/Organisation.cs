using CodeCrafters.Domain.Entities;

namespace CodeCrafters.Domain.Entities.Organisations;

/// <summary>
/// Represents an applicant organisation's profile. One per Applicant user.
/// </summary>
public class Organisation
{
    public Guid Id { get; set; }

    /// <summary>Foreign key to the Applicant user who owns this profile.</summary>
    public Guid UserId { get; set; }
    public User? User { get; set; }

    public string Name { get; set; } = string.Empty;
    public string RegistrationNumber { get; set; } = string.Empty;

    /// <summary>Type stored as string e.g. NGO, Trust, Section8Company, Society, etc.</summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>Indian state name.</summary>
    public string State { get; set; } = string.Empty;

    /// <summary>Annual budget in INR.</summary>
    public decimal AnnualBudget { get; set; }

    public string ContactPersonName { get; set; } = string.Empty;
    public string ContactPersonEmail { get; set; } = string.Empty;
    public string ContactPersonPhone { get; set; } = string.Empty;

    public int YearOfEstablishment { get; set; }

    /// <summary>True once all mandatory fields have been submitted.</summary>
    public bool IsProfileComplete { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
