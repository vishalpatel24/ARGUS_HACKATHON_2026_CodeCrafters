namespace CodeCrafters.Application.Screening.Dtos;

public class ScreeningReportDto
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string ApplicationReference { get; set; } = string.Empty;
    public string ApplicationTitle { get; set; } = string.Empty;
    public string GrantTypeCode { get; set; } = string.Empty;
    public string ApplicantName { get; set; } = string.Empty;
    public string OrganisationName { get; set; } = string.Empty;

    public string OverallResult { get; set; } = string.Empty;
    public int HardChecksPassed { get; set; }
    public int HardChecksFailed { get; set; }
    public int SoftFlags { get; set; }

    public string? OfficerAction { get; set; }
    public string? OfficerActionReason { get; set; }
    public string? ReviewedByUserName { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public DateTime GeneratedAt { get; set; }

    public List<ScreeningCheckDto> Checks { get; set; } = new();
}
