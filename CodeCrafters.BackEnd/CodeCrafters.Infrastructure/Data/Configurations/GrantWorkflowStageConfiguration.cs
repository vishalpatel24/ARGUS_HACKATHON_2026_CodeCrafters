using CodeCrafters.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeCrafters.Infrastructure.Data.Configurations;

public class GrantWorkflowStageConfiguration : IEntityTypeConfiguration<GrantWorkflowStage>
{
    // Fixed GrantType GUIDs from GrantTypeConfiguration
    private static readonly Guid CdgId  = Guid.Parse("A1111111-1111-1111-1111-111111111111");
    private static readonly Guid EigId  = Guid.Parse("A2222222-2222-2222-2222-222222222222");
    private static readonly Guid EcagId = Guid.Parse("A3333333-3333-3333-3333-333333333333");

    private static readonly DateTime SeedDate = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

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

        // ─── CDG Stages ─────────────────────────────────────────────────────────
        builder.HasData(
            Stage("C1111111-0001-0001-0001-111111111111", CdgId, 1, "Submitted",             "System",           0, null, "Instant"),
            Stage("C1111111-0001-0001-0002-111111111111", CdgId, 2, "Eligibility Screening", "AI Agent",         0,  1,   "Working Days"),
            Stage("C1111111-0001-0001-0003-111111111111", CdgId, 3, "Under Review",          "Program Officer",  1, 14,   "Working Days"),
            Stage("C1111111-0001-0001-0004-111111111111", CdgId, 4, "Award Decision",        "Program Officer",  0,  7,   "Working Days"),
            Stage("C1111111-0001-0001-0005-111111111111", CdgId, 5, "Agreement Sent",        "Program Officer",  0,  5,   "Working Days"),
            Stage("C1111111-0001-0001-0006-111111111111", CdgId, 6, "Active Grant",          "Applicant",        0, null, "N/A"),
            Stage("C1111111-0001-0001-0007-111111111111", CdgId, 7, "Reporting",             "Applicant",        0, 30,   "Calendar Days"),
            Stage("C1111111-0001-0001-0008-111111111111", CdgId, 8, "Closed",                "System",           0, null, "Instant"),

        // ─── EIG Stages ─────────────────────────────────────────────────────────
            Stage("E2222222-0002-0002-0001-222222222222", EigId, 1, "Submitted",             "System",           0, null, "Instant"),
            Stage("E2222222-0002-0002-0002-222222222222", EigId, 2, "Eligibility Screening", "AI Agent",         0,  1,   "Working Days"),
            Stage("E2222222-0002-0002-0003-222222222222", EigId, 3, "Under Review",          "Grant Reviewer",   2, 21,   "Working Days"),
            Stage("E2222222-0002-0002-0004-222222222222", EigId, 4, "Award Decision",        "Program Officer",  0,  7,   "Working Days"),
            Stage("E2222222-0002-0002-0005-222222222222", EigId, 5, "Agreement Sent",        "Program Officer",  0,  5,   "Working Days"),
            Stage("E2222222-0002-0002-0006-222222222222", EigId, 6, "Active Grant",          "Applicant",        0, null, "N/A"),
            Stage("E2222222-0002-0002-0007-222222222222", EigId, 7, "Reporting",             "Applicant",        0, 30,   "Calendar Days"),
            Stage("E2222222-0002-0002-0008-222222222222", EigId, 8, "Closed",                "System",           0, null, "Instant"),

        // ─── ECAG Stages ─────────────────────────────────────────────────────────
            Stage("F3333333-0003-0003-0001-333333333333", EcagId, 1, "Submitted",             "System",           0, null, "Instant"),
            Stage("F3333333-0003-0003-0002-333333333333", EcagId, 2, "Eligibility Screening", "AI Agent",         0,  1,   "Working Days"),
            Stage("F3333333-0003-0003-0003-333333333333", EcagId, 3, "Under Review",          "Program Officer",  1, 14,   "Working Days"),
            Stage("F3333333-0003-0003-0004-333333333333", EcagId, 4, "Award Decision",        "Program Officer",  0,  7,   "Working Days"),
            Stage("F3333333-0003-0003-0005-333333333333", EcagId, 5, "Agreement Sent",        "Program Officer",  0,  5,   "Working Days"),
            Stage("F3333333-0003-0003-0006-333333333333", EcagId, 6, "Active Grant",          "Applicant",        0, null, "N/A"),
            Stage("F3333333-0003-0003-0007-333333333333", EcagId, 7, "Reporting",             "Applicant",        0, 30,   "Calendar Days"),
            Stage("F3333333-0003-0003-0008-333333333333", EcagId, 8, "Closed",                "System",           0, null, "Instant")
        );
    }

    private static GrantWorkflowStage Stage(
        string id, Guid grantTypeId, int order, string name,
        string assigneeRole, int requiredReviewers, int? slaDays, string slaType)
        => new()
        {
            Id = Guid.Parse(id),
            GrantTypeId = grantTypeId,
            OrderIdx = order,
            Name = name,
            AssigneeRole = assigneeRole,
            RequiredReviewers = requiredReviewers,
            SlaDays = slaDays,
            SlaType = slaType,
            CreatedAt = SeedDate,
            UpdatedAt = SeedDate,
            IsDeleted = false
        };
}
