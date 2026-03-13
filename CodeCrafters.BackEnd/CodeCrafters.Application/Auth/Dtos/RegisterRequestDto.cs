namespace CodeCrafters.Application.Auth.Dtos;

public sealed class RegisterRequestDto
{
    public string Name { get; init; } = string.Empty;

    public string OrganisationName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string? Phone { get; init; }
}
