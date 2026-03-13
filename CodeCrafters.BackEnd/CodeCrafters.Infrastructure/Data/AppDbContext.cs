using CodeCrafters.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CodeCrafters.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<SampleEntity> SampleEntities => Set<SampleEntity>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Name).IsRequired().HasMaxLength(200);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Phone).HasMaxLength(20);
            entity.Property(u => u.Role).HasConversion<string>().HasMaxLength(50);
            entity.Property(u => u.IsActive).HasDefaultValue(true);
        });
    }
}

