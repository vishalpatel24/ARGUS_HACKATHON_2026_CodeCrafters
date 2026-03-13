namespace CodeCrafters.Domain.Entities.Grants;

/// <summary>
/// Stores the Programme Summary attributes for each grant programme (CDG, EIG, ECAG).
/// </summary>
public class GrantType
{
    public Guid Id { get; set; }

    /// <summary>Short identifier (e.g. CDG, EIG, ECAG).</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>Full grant programme name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Description of the programme purpose.</summary>
    public string Purpose { get; set; } = string.Empty;

    public decimal FundingMinAmount { get; set; }
    public decimal FundingMaxAmount { get; set; }

    public int ProjectDurationMinMonths { get; set; }
    public int ProjectDurationMaxMonths { get; set; }

    /// <summary>Text describing which organisations can apply.</summary>
    public string EligibleApplicants { get; set; } = string.Empty;

    public string GeographicFocus { get; set; } = string.Empty;

    /// <summary>Text description of the application cycle.</summary>
    public string AnnualCycle { get; set; } = string.Empty;

    public int MaximumAwardsPerCycle { get; set; }
    public decimal TotalProgrammeBudget { get; set; }

    public ICollection<GrantWorkflowStage> WorkflowStages { get; set; } = new List<GrantWorkflowStage>();

    public ICollection<GrantDocument> RequiredDocuments { get; set; } = new List<GrantDocument>();
}
