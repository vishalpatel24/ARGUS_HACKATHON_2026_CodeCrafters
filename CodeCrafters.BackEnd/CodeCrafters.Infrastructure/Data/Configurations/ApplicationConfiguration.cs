using CodeCrafters.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeCrafters.Infrastructure.Data.Configurations;

public class ApplicationConfiguration : IEntityTypeConfiguration<CodeCrafters.Domain.Entities.Applications.Application>
{
    public void Configure(EntityTypeBuilder<CodeCrafters.Domain.Entities.Applications.Application> builder)
    {
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.ReferenceNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.StatusLabel)
            .HasMaxLength(100);

        builder.Property(a => a.Title)
            .HasMaxLength(200);

        builder.Property(a => a.RequestedAmount).HasColumnType("decimal(18,2)");
        builder.Property(a => a.TotalRequestedAmount).HasColumnType("decimal(18,2)");
        builder.Property(a => a.AnnualOperatingBudget).HasColumnType("decimal(18,2)");
        builder.Property(a => a.PersonnelCosts).HasColumnType("decimal(18,2)");
        builder.Property(a => a.EquipmentAndMaterials).HasColumnType("decimal(18,2)");
        builder.Property(a => a.TravelAndLogistics).HasColumnType("decimal(18,2)");
        builder.Property(a => a.TrainingAndWorkshops).HasColumnType("decimal(18,2)");
        builder.Property(a => a.TechnologySoftware).HasColumnType("decimal(18,2)");
        builder.Property(a => a.ContentDevelopment).HasColumnType("decimal(18,2)");
        builder.Property(a => a.SaplingsAndSeeds).HasColumnType("decimal(18,2)");
        builder.Property(a => a.CommunityEngagementWages).HasColumnType("decimal(18,2)");
        builder.Property(a => a.Overheads).HasColumnType("decimal(18,2)");
        builder.Property(a => a.OtherCosts).HasColumnType("decimal(18,2)");

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
