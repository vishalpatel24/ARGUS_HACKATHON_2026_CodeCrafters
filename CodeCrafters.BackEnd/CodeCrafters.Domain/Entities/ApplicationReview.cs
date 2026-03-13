using CodeCrafters.Domain.Common;

namespace CodeCrafters.Domain.Entities;

public class ApplicationReview : AuditableEntity
{
    public Guid ApplicationId { get; set; }
    public Guid ReviewerId { get; set; }
    
    public int? AiSuggestedScore { get; set; }
    public int? FinalScore { get; set; }
    public string ReviewStatus { get; set; } = "Pending";
    public DateTimeOffset? SubmittedAt { get; set; }
    
    public Applications.Application Application { get; set; } = null!;
    public User Reviewer { get; set; } = null!;
}

