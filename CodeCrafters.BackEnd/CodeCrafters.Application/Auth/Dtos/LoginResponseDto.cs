namespace CodeCrafters.Application.Auth.Dtos;

public sealed class LoginResponseDto
{
    public string Token { get; init; } = string.Empty;

    public UserResponseDto User { get; init; } = null!;
}
