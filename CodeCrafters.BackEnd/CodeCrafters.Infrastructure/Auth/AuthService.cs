using CodeCrafters.Application.Auth.Dtos;
using CodeCrafters.Application.Auth.Services;
using CodeCrafters.Domain.Entities;
using CodeCrafters.Domain.Enums;
using CodeCrafters.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeCrafters.Infrastructure.Auth;

public sealed class AuthService(
    AppDbContext dbContext,
    JwtTokenService jwtTokenService,
    ILogger<AuthService> logger) : IAuthService
{
    public async Task<LoginResponseDto> RegisterAsync(RegisterRequestDto dto, CancellationToken cancellationToken = default)
    {
        var emailExists = await dbContext.Users
            .AnyAsync(u => u.Email == dto.Email, cancellationToken);

        if (emailExists)
            throw new InvalidOperationException("A user with this email already exists.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            OrganisationName = dto.OrganisationName?.Trim().Length > 0 ? dto.OrganisationName : null,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Phone = dto.Phone,
            Role = UserRole.Applicant,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("New applicant registered: {Email}", user.Email);

        var token = jwtTokenService.GenerateToken(user);

        return new LoginResponseDto
        {
            Token = token,
            User = MapToDto(user)
        };
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto, CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == dto.Email, cancellationToken);

        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is deactivated. Contact administrator.");

        logger.LogInformation("User logged in: {Email}", user.Email);

        var token = jwtTokenService.GenerateToken(user);

        return new LoginResponseDto
        {
            Token = token,
            User = MapToDto(user)
        };
    }

    private static UserResponseDto MapToDto(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        OrganisationName = user.OrganisationName,
        Email = user.Email,
        Phone = user.Phone,
        Role = user.Role.ToString(),
        IsActive = user.IsActive,
        CreatedAt = user.CreatedAt
    };
}
