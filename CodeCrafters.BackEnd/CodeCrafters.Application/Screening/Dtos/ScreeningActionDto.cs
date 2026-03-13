using System.ComponentModel.DataAnnotations;

namespace CodeCrafters.Application.Screening.Dtos;

public class ScreeningActionDto
{
    /// <summary>ConfirmEligible, OverrideIneligible, ClarificationRequested</summary>
    [Required]
    public string Action { get; set; } = string.Empty;

    /// <summary>Required for OverrideIneligible and ClarificationRequested</summary>
    public string? Reason { get; set; }
}
