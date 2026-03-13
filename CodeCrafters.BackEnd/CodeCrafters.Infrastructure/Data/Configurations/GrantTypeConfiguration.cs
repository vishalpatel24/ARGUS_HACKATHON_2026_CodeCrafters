using CodeCrafters.Domain.Entities.Grants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeCrafters.Infrastructure.Data.Configurations;

public class GrantTypeConfiguration : IEntityTypeConfiguration<GrantType>
{
    public void Configure(EntityTypeBuilder<GrantType> builder)
    {
        builder.ToTable("GrantTypes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(10);
        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Purpose)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.FundingMinAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.FundingMaxAmount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.ProjectDurationMinMonths)
            .IsRequired();

        builder.Property(x => x.ProjectDurationMaxMonths)
            .IsRequired();

        builder.Property(x => x.EligibleApplicants)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.GeographicFocus)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.AnnualCycle)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(x => x.MaximumAwardsPerCycle)
            .IsRequired();

        builder.Property(x => x.TotalProgrammeBudget)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.HasData(
            new GrantType
            {
                Id = Guid.Parse("A1111111-1111-1111-1111-111111111111"),
                Code = "CDG",
                Name = "Community Development Grant",
                Purpose = "Fund community-level infrastructure and social service projects",
                FundingMinAmount = 200000m,
                FundingMaxAmount = 2000000m,
                ProjectDurationMinMonths = 6,
                ProjectDurationMaxMonths = 18,
                EligibleApplicants = "Registered NGOs, Trusts, Section 8 Companies with minimum 2 years of operation",
                GeographicFocus = "Rural and semi-urban areas in India",
                AnnualCycle = "Applications open April 1 – June 30 each year",
                MaximumAwardsPerCycle = 10,
                TotalProgrammeBudget = 20000000m
            },
            new GrantType
            {
                Id = Guid.Parse("A2222222-2222-2222-2222-222222222222"),
                Code = "EIG",
                Name = "Education Innovation Grant",
                Purpose = "Fund technology-enabled or pedagogy-innovation projects improving learning outcomes in government schools",
                FundingMinAmount = 500000m,
                FundingMaxAmount = 5000000m,
                ProjectDurationMinMonths = 12,
                ProjectDurationMaxMonths = 24,
                EligibleApplicants = "NGOs, EdTech non-profits, Research institutions, Universities (public or private)",
                GeographicFocus = "Any state in India; preference for aspirational districts",
                AnnualCycle = "Rolling applications — reviewed quarterly (Jan, Apr, Jul, Oct)",
                MaximumAwardsPerCycle = 5,
                TotalProgrammeBudget = 25000000m
            },
            new GrantType
            {
                Id = Guid.Parse("A3333333-3333-3333-3333-333333333333"),
                Code = "ECAG",
                Name = "Environment & Climate Action Grant",
                Purpose = "Fund grassroots environmental conservation, climate resilience, and clean energy access projects",
                FundingMinAmount = 300000m,
                FundingMaxAmount = 3000000m,
                ProjectDurationMinMonths = 6,
                ProjectDurationMaxMonths = 24,
                EligibleApplicants = "NGOs, Farmer Producer Organisations (FPOs), Panchayat bodies, Research institutions",
                GeographicFocus = "India — priority given to climate-vulnerable districts",
                AnnualCycle = "Applications open twice yearly: Jan 1–Feb 28 and Jul 1–Aug 31",
                MaximumAwardsPerCycle = 15,
                TotalProgrammeBudget = 30000000m
            }
        );
    }
}
