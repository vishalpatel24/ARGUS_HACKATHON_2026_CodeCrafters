using CodeCrafters.Domain.Common;
using CodeCrafters.Domain.Entities.Grants;

namespace CodeCrafters.Domain.Entities.Applications;

public class ApplicationWorkflowHistory : AuditableEntity
{
    public Guid ApplicationId { get; set; }
    public Guid StageId { get; set; }
    public DateTimeOffset TransitionedAt { get; set; }
    public Guid? TriggeredByUserId { get; set; }
    public bool IsAiTriggered { get; set; }
    public string StatusLabel { get; set; } = string.Empty;
    public string? ActionNotes { get; set; }
    
    public Application Application { get; set; } = null!;
    public GrantWorkflowStage Stage { get; set; } = null!;
    public User? TriggeredByUser { get; set; }
}
