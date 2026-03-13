using CodeCrafters.Domain.Entities.Applications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeCrafters.Infrastructure.Data.Configurations;

public class ApplicationWorkflowHistoryConfiguration : IEntityTypeConfiguration<ApplicationWorkflowHistory>
{
    public void Configure(EntityTypeBuilder<ApplicationWorkflowHistory> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.StatusLabel)
            .HasMaxLength(100);

        builder.HasOne(a => a.Application)
            .WithMany(app => app.WorkflowHistories)
            .HasForeignKey(a => a.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Stage)
            .WithMany()
            .HasForeignKey(a => a.StageId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(a => a.TriggeredByUser)
            .WithMany()
            .HasForeignKey(a => a.TriggeredByUserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
