using CodeCrafters.Domain.Entities.Grants;
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
    }
}
