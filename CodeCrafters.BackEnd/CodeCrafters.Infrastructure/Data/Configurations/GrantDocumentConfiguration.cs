using CodeCrafters.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeCrafters.Infrastructure.Data.Configurations;

public class GrantDocumentConfiguration : IEntityTypeConfiguration<GrantDocument>
{
    public void Configure(EntityTypeBuilder<GrantDocument> builder)
    {
        builder.ToTable("GrantDocuments");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.GrantTypeId)
            .IsRequired();

        builder.Property(d => d.DocumentName)
            .IsRequired();

        builder.HasIndex(d => d.GrantTypeId);

        builder.HasOne(d => d.GrantType)
            .WithMany(g => g.RequiredDocuments)
            .HasForeignKey(d => d.GrantTypeId);

        builder.HasData(
            // CDG documents
            new GrantDocument
            {
                Id = Guid.Parse("B1111111-1111-1111-1111-111111111111"),
                GrantTypeId = Guid.Parse("A1111111-1111-1111-1111-111111111111"),
                DocumentName = "Organization Registration Certificate",
                IsMandatory = true,
                DisplayOrder = 1
            },
            new GrantDocument
            {
                Id = Guid.Parse("B1111111-1111-1111-1111-222222222222"),
                GrantTypeId = Guid.Parse("A1111111-1111-1111-1111-111111111111"),
                DocumentName = "Latest Audited Financial Statements",
                IsMandatory = true,
                DisplayOrder = 2
            },
            new GrantDocument
            {
                Id = Guid.Parse("B1111111-1111-1111-1111-333333333333"),
                GrantTypeId = Guid.Parse("A1111111-1111-1111-1111-111111111111"),
                DocumentName = "Project Proposal Document",
                IsMandatory = true,
                DisplayOrder = 3
            },
            new GrantDocument
            {
                Id = Guid.Parse("B1111111-1111-1111-1111-444444444444"),
                GrantTypeId = Guid.Parse("A1111111-1111-1111-1111-111111111111"),
                DocumentName = "Implementation Plan",
                IsMandatory = true,
                DisplayOrder = 4
            },
            new GrantDocument
            {
                Id = Guid.Parse("B1111111-1111-1111-1111-555555555555"),
                GrantTypeId = Guid.Parse("A1111111-1111-1111-1111-111111111111"),
                DocumentName = "Budget Breakdown",
                IsMandatory = true,
                DisplayOrder = 5
            },
            new GrantDocument
            {
                Id = Guid.Parse("B1111111-1111-1111-1111-666666666666"),
                GrantTypeId = Guid.Parse("A1111111-1111-1111-1111-111111111111"),
                DocumentName = "Letters of Community Support",
                IsMandatory = false,
                DisplayOrder = 6
            },

            // EIG documents
            new GrantDocument
            {
                Id = Guid.Parse("B2222222-2222-2222-2222-111111111111"),
                GrantTypeId = Guid.Parse("A2222222-2222-2222-2222-222222222222"),
                DocumentName = "Organization Registration Certificate",
                IsMandatory = true,
                DisplayOrder = 1
            },
            new GrantDocument
            {
                Id = Guid.Parse("B2222222-2222-2222-2222-222222222222"),
                GrantTypeId = Guid.Parse("A2222222-2222-2222-2222-222222222222"),
                DocumentName = "Project Concept Note",
                IsMandatory = true,
                DisplayOrder = 2
            },
            new GrantDocument
            {
                Id = Guid.Parse("B2222222-2222-2222-2222-333333333333"),
                GrantTypeId = Guid.Parse("A2222222-2222-2222-2222-222222222222"),
                DocumentName = "Detailed Project Proposal",
                IsMandatory = true,
                DisplayOrder = 3
            },
            new GrantDocument
            {
                Id = Guid.Parse("B2222222-2222-2222-2222-444444444444"),
                GrantTypeId = Guid.Parse("A2222222-2222-2222-2222-222222222222"),
                DocumentName = "Technology Solution Description",
                IsMandatory = true,
                DisplayOrder = 4
            },
            new GrantDocument
            {
                Id = Guid.Parse("B2222222-2222-2222-2222-555555555555"),
                GrantTypeId = Guid.Parse("A2222222-2222-2222-2222-222222222222"),
                DocumentName = "Budget Plan",
                IsMandatory = true,
                DisplayOrder = 5
            },
            new GrantDocument
            {
                Id = Guid.Parse("B2222222-2222-2222-2222-666666666666"),
                GrantTypeId = Guid.Parse("A2222222-2222-2222-2222-222222222222"),
                DocumentName = "Past Impact Evidence",
                IsMandatory = false,
                DisplayOrder = 6
            },

            // ECAG documents
            new GrantDocument
            {
                Id = Guid.Parse("B3333333-3333-3333-3333-111111111111"),
                GrantTypeId = Guid.Parse("A3333333-3333-3333-3333-333333333333"),
                DocumentName = "Organization Registration Certificate",
                IsMandatory = true,
                DisplayOrder = 1
            },
            new GrantDocument
            {
                Id = Guid.Parse("B3333333-3333-3333-3333-222222222222"),
                GrantTypeId = Guid.Parse("A3333333-3333-3333-3333-333333333333"),
                DocumentName = "Environmental Impact Description",
                IsMandatory = true,
                DisplayOrder = 2
            },
            new GrantDocument
            {
                Id = Guid.Parse("B3333333-3333-3333-3333-333333333333"),
                GrantTypeId = Guid.Parse("A3333333-3333-3333-3333-333333333333"),
                DocumentName = "Project Proposal",
                IsMandatory = true,
                DisplayOrder = 3
            },
            new GrantDocument
            {
                Id = Guid.Parse("B3333333-3333-3333-3333-444444444444"),
                GrantTypeId = Guid.Parse("A3333333-3333-3333-3333-333333333333"),
                DocumentName = "Implementation Timeline",
                IsMandatory = true,
                DisplayOrder = 4
            },
            new GrantDocument
            {
                Id = Guid.Parse("B3333333-3333-3333-3333-555555555555"),
                GrantTypeId = Guid.Parse("A3333333-3333-3333-3333-333333333333"),
                DocumentName = "Budget Plan",
                IsMandatory = true,
                DisplayOrder = 5
            },
            new GrantDocument
            {
                Id = Guid.Parse("B3333333-3333-3333-3333-666666666666"),
                GrantTypeId = Guid.Parse("A3333333-3333-3333-3333-333333333333"),
                DocumentName = "Community Partnership Letters",
                IsMandatory = false,
                DisplayOrder = 6
            }
        );
    }
}

