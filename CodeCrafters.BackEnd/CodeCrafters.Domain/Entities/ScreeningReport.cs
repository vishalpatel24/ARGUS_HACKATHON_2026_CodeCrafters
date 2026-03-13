using CodeCrafters.Domain.Common;

namespace CodeCrafters.Domain.Entities;

public class ScreeningReport : AuditableEntity
{
    public Guid ApplicationId { get; set; }
    public Applications.Application Application { get; set; } = null!;

    /// <summary>Overall result: Eligible, Ineligible, PendingReview</summary>
    public string OverallResult { get; set; } = "PendingReview";

    public int HardChecksPassed { get; set; }
    public int HardChecksFailed { get; set; }
    public int SoftFlags { get; set; }

    /// <summary>Program Officer action: null (pending), ConfirmEligible, OverrideIneligible, ClarificationRequested</summary>
    public string? OfficerAction { get; set; }
    public string? OfficerActionReason { get; set; }
    public Guid? ReviewedByUserId { get; set; }
    public User? ReviewedByUser { get; set; }
    public DateTime? ReviewedAt { get; set; }

    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ScreeningCheck> Checks { get; set; } = new List<ScreeningCheck>();
}
