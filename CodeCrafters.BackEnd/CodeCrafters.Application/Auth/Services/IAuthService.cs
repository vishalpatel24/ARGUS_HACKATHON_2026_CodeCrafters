using CodeCrafters.Application.Auth.Dtos;

namespace CodeCrafters.Application.Auth.Services;

public interface IAuthService
{
    Task<LoginResponseDto> RegisterAsync(RegisterRequestDto dto, CancellationToken cancellationToken = default);

    Task<LoginResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default);
}
