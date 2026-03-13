namespace CodeCrafters.Application.Auth.Dtos;

public sealed class CreateUserDto
{
    public string Name { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string? Phone { get; init; }

    public string Role { get; init; } = string.Empty;
}
