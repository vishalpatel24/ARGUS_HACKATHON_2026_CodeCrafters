namespace CodeCrafters.Application.Screening.Dtos;

public class ScreeningCheckDto
{
    public Guid Id { get; set; }
    public string CheckCode { get; set; } = string.Empty;
    public string CheckName { get; set; } = string.Empty;
    public string CheckType { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public string? Details { get; set; }
    public int? AiScore { get; set; }
    public int DisplayOrder { get; set; }
}
