using CodeCrafters.Domain.Entities.Applications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeCrafters.Infrastructure.Data.Configurations;

public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.ReferenceNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.StatusLabel)
            .HasMaxLength(100);

        builder.Property(a => a.Title)
            .HasMaxLength(200);

        builder.Property(a => a.RequestedAmount)
            .HasColumnType("decimal(18,2)");

        builder.HasOne(a => a.CurrentStage)
            .WithMany()
            .HasForeignKey(a => a.CurrentStageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.GrantType)
            .WithMany()
            .HasForeignKey(a => a.GrantTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.Applicant)
            .WithMany()
            .HasForeignKey(a => a.ApplicantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
