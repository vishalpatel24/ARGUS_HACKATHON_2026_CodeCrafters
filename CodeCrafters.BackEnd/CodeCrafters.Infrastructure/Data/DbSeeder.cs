using CodeCrafters.Domain.Entities;
using CodeCrafters.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CodeCrafters.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext dbContext)
    {
        var adminExists = await dbContext.Users
            .AnyAsync(u => u.Role == UserRole.PlatformAdmin);

        if (adminExists) return;

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Name = "Platform Admin",
            Email = "admin@grantflow.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Phone = null,
            Role = UserRole.PlatformAdmin,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        dbContext.Users.Add(admin);
        await dbContext.SaveChangesAsync();
    }
}
