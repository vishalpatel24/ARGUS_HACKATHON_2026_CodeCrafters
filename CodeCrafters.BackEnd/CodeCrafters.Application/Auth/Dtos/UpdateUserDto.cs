namespace CodeCrafters.Application.Auth.Dtos;

public sealed class UpdateUserDto
{
    public string? Name { get; init; }

    public string? Email { get; init; }

    public string? Phone { get; init; }

    public string? Role { get; init; }
}
