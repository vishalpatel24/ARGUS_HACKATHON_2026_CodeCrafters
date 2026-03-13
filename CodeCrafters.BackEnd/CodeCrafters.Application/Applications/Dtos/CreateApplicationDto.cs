namespace CodeCrafters.Application.Applications.Dtos;

public sealed class CreateApplicationDto
{
    public Guid GrantTypeId { get; init; }
    public string Title { get; init; } = string.Empty;
}
