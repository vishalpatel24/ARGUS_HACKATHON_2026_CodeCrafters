using CodeCrafters.Domain.Entities.Applications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeCrafters.Infrastructure.Data.Configurations;

public class ApplicationReviewConfiguration : IEntityTypeConfiguration<ApplicationReview>
{
    public void Configure(EntityTypeBuilder<ApplicationReview> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.ReviewStatus)
            .HasMaxLength(50);

        builder.HasOne(a => a.Application)
            .WithMany()
            .HasForeignKey(a => a.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Reviewer)
            .WithMany()
            .HasForeignKey(a => a.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
