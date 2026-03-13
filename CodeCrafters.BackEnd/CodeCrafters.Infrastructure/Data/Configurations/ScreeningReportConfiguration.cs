using CodeCrafters.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeCrafters.Infrastructure.Data.Configurations;

public class ScreeningReportConfiguration : IEntityTypeConfiguration<ScreeningReport>
{
    public void Configure(EntityTypeBuilder<ScreeningReport> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.OverallResult).IsRequired().HasMaxLength(50);
        builder.Property(s => s.OfficerAction).HasMaxLength(50);
        builder.Property(s => s.OfficerActionReason).HasMaxLength(2000);

        builder.HasOne(s => s.Application)
            .WithMany()
            .HasForeignKey(s => s.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.ReviewedByUser)
            .WithMany()
            .HasForeignKey(s => s.ReviewedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.Checks)
            .WithOne(c => c.ScreeningReport)
            .HasForeignKey(c => c.ScreeningReportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
