using CodeCrafters.Application.Auth.Dtos;

namespace CodeCrafters.Application.Auth.Services;

public interface IUserService
{
    Task<UserResponseDto> CreateUserAsync(CreateUserDto dto, CancellationToken cancellationToken = default);

    Task<List<UserResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);

    Task<UserResponseDto?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<UserResponseDto> UpdateUserAsync(Guid id, UpdateUserDto dto, CancellationToken cancellationToken = default);

    Task DeactivateUserAsync(Guid id, CancellationToken cancellationToken = default);
}
