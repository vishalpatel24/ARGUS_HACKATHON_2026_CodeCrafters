namespace CodeCrafters.Application.Applications.Dtos;

public sealed class ApplicationListItemDto
{
    public Guid Id { get; init; }
    public string ReferenceNumber { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string GrantTypeName { get; init; } = string.Empty;
    public string StatusLabel { get; init; } = string.Empty;
    public string CurrentStageName { get; init; } = string.Empty;
    public DateTimeOffset LastUpdated { get; init; }
    public DateTimeOffset? SubmissionDate { get; init; }
}
