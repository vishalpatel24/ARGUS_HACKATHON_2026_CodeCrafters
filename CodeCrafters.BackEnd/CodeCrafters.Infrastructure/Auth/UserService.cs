using CodeCrafters.Application.Auth.Dtos;
using CodeCrafters.Application.Auth.Services;
using CodeCrafters.Domain.Entities;
using CodeCrafters.Domain.Enums;
using CodeCrafters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeCrafters.Infrastructure.Auth;

public sealed class UserService(
    AppDbContext dbContext,
    ILogger<UserService> logger) : IUserService
{
    public async Task<UserResponseDto> CreateUserAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
    {
        var emailExists = await dbContext.Users
            .AnyAsync(u => u.Email == dto.Email, cancellationToken);

        if (emailExists)
            throw new InvalidOperationException("A user with this email already exists.");

        if (!Enum.TryParse<UserRole>(dto.Role, ignoreCase: true, out var role))
            throw new ArgumentException($"Invalid role: {dto.Role}");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Phone = dto.Phone,
            Role = role,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("User created by admin: {Email} with role {Role}", user.Email, user.Role);

        return MapToDto(user);
    }

    public async Task<List<UserResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await dbContext.Users
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync(cancellationToken);

        return users.Select(MapToDto).ToList();
    }

    public async Task<UserResponseDto?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        return user is null ? null : MapToDto(user);
    }

    public async Task<UserResponseDto> UpdateUserAsync(Guid id, UpdateUserDto dto, CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"User with ID {id} not found.");

        if (dto.Name is not null) user.Name = dto.Name;

        if (dto.Email is not null)
        {
            var emailTaken = await dbContext.Users
                .AnyAsync(u => u.Email == dto.Email && u.Id != id, cancellationToken);

            if (emailTaken)
                throw new InvalidOperationException("A user with this email already exists.");

            user.Email = dto.Email;
        }

        if (dto.Phone is not null) user.Phone = dto.Phone;

        if (dto.Role is not null)
        {
            if (!Enum.TryParse<UserRole>(dto.Role, ignoreCase: true, out var role))
                throw new ArgumentException($"Invalid role: {dto.Role}");

            user.Role = role;
        }

        user.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("User updated: {UserId}", id);

        return MapToDto(user);
    }

    public async Task DeactivateUserAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"User with ID {id} not found.");

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("User deactivated: {UserId}", id);
    }

    private static UserResponseDto MapToDto(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
        Phone = user.Phone,
        Role = user.Role.ToString(),
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt
    };
}
