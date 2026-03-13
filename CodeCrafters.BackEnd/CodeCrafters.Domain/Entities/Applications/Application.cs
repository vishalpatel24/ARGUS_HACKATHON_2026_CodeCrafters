using CodeCrafters.Domain.Common;
using CodeCrafters.Domain.Entities.Grants;

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

    public ICollection<ApplicationWorkflowHistory> WorkflowHistories { get; set; } = new List<ApplicationWorkflowHistory>();
}
