namespace CodeCrafters.Application.Auth.Dtos;

public sealed class UserResponseDto
{
    public Guid Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? OrganisationName { get; init; }

    public string Email { get; init; } = string.Empty;

    public string? Phone { get; init; }

    public string Role { get; init; } = string.Empty;

    public bool IsActive { get; init; }

    public DateTime CreatedAt { get; init; }
}
