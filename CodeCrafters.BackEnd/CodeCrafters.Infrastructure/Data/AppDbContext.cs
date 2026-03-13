using CodeCrafters.Domain.Entities;
using CodeCrafters.Domain.Entities.Grants;
using CodeCrafters.Domain.Entities.Applications;
using CodeCrafters.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CodeCrafters.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<SampleEntity> SampleEntities => Set<SampleEntity>();

    public DbSet<User> Users => Set<User>();

    public DbSet<GrantType> GrantTypes => Set<GrantType>();

    public DbSet<GrantWorkflowStage> GrantWorkflowStages => Set<GrantWorkflowStage>();

    public DbSet<GrantDocument> GrantDocuments => Set<GrantDocument>();

    public DbSet<Application> Applications => Set<Application>();

    public DbSet<ApplicationWorkflowHistory> ApplicationWorkflowHistories => Set<ApplicationWorkflowHistory>();

    public DbSet<ApplicationReview> ApplicationReviews => Set<ApplicationReview>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new GrantTypeConfiguration());
        modelBuilder.ApplyConfiguration(new GrantWorkflowStageConfiguration());
        modelBuilder.ApplyConfiguration(new GrantDocumentConfiguration());
        modelBuilder.ApplyConfiguration(new ApplicationConfiguration());
        modelBuilder.ApplyConfiguration(new ApplicationWorkflowHistoryConfiguration());
        modelBuilder.ApplyConfiguration(new ApplicationReviewConfiguration());

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

