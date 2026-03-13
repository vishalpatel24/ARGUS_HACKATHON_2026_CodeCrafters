namespace CodeCrafters.Domain.Entities;

public class ScreeningCheck
{
    public Guid Id { get; set; }

    public Guid ScreeningReportId { get; set; }
    public ScreeningReport ScreeningReport { get; set; } = null!;

    /// <summary>E.g. E1, E2, ... E9, SoftCheck1, SoftCheck2</summary>
    public string CheckCode { get; set; } = string.Empty;

    public string CheckName { get; set; } = string.Empty;

    /// <summary>Hard or Soft</summary>
    public string CheckType { get; set; } = "Hard";

    /// <summary>Pass, Fail, Flag</summary>
    public string Result { get; set; } = string.Empty;

    public string? Details { get; set; }

    /// <summary>For AI soft checks — the score (0-100)</summary>
    public int? AiScore { get; set; }

    public int DisplayOrder { get; set; }
}
