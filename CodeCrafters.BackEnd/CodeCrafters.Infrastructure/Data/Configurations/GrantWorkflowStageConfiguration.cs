using CodeCrafters.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeCrafters.Infrastructure.Data.Configurations;

public class GrantWorkflowStageConfiguration : IEntityTypeConfiguration<GrantWorkflowStage>
{
    public void Configure(EntityTypeBuilder<GrantWorkflowStage> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(g => g.AssigneeRole)
            .HasMaxLength(50);

        builder.Property(g => g.SlaType)
            .HasMaxLength(50);

        builder.HasOne(g => g.GrantType)
            .WithMany(gt => gt.WorkflowStages)
            .HasForeignKey(g => g.GrantTypeId)
            .OnDelete(DeleteBehavior.Cascade);

        // -- Seeding Stages for CDG --
        SeedStages(builder, Guid.Parse("A1111111-1111-1111-1111-111111111111"), 100);
        
        // -- Seeding Stages for EIG --
        SeedStages(builder, Guid.Parse("A2222222-2222-2222-2222-222222222222"), 200);

        // -- Seeding Stages for ECAG --
        SeedStages(builder, Guid.Parse("A3333333-3333-3333-3333-333333333333"), 300);
    }

    private static void SeedStages(EntityTypeBuilder<GrantWorkflowStage> builder, Guid grantTypeId, int startId)
    {
        string[] stages = { "Submitted", "Eligibility Screening", "Under Review", "Award Decision", "Agreement Sent", "Active Grant", "Reporting", "Closed" };
        string[] roles = { "Program Officer", "Program Officer", "Grant Reviewer", "Grant Admin", "Grant Admin", "Finance Officer", "Program Officer", "Grant Admin" };

        for (int i = 0; i < stages.Length; i++)
        {
            builder.HasData(new GrantWorkflowStage
            {
                Id = Guid.Parse($"EEEEE111-1111-1111-1111-{startId + i:D12}"),
                GrantTypeId = grantTypeId,
                Name = stages[i],
                OrderIdx = i + 1,
                AssigneeRole = roles[i],
                RequiredReviewers = (stages[i] == "Under Review") ? 2 : 0,
                SlaDays = 7,
                SlaType = "Working Days",
                CreatedAt = new DateTime(2026, 3, 13)
            });
        }
    }
}
