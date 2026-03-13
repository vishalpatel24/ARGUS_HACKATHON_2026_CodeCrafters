using CodeCrafters.Domain.Common;

namespace CodeCrafters.Domain.Entities.Grants;

public class GrantWorkflowStage : AuditableEntity
{
    public Guid GrantTypeId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public int OrderIdx { get; set; }
    
    public string AssigneeRole { get; set; } = string.Empty;
    public int RequiredReviewers { get; set; }
    public int? SlaDays { get; set; }
    public string SlaType { get; set; } = "Working Days";

    public GrantType GrantType { get; set; } = null!;
}
