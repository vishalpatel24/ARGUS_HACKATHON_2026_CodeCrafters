using CodeCrafters.Domain.Entities.Organisation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeCrafters.Infrastructure.Data.Configurations;

public class OrganisationConfiguration : IEntityTypeConfiguration<Organisation>
{
    public void Configure(EntityTypeBuilder<Organisation> builder)
    {
        builder.ToTable("Organisations");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(o => o.OrganisationType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(100);

        builder.Property(o => o.RegistrationNumber)
            .HasMaxLength(100);

        builder.Property(o => o.YearEstablished);

        builder.Property(o => o.AddressLine1)
            .HasMaxLength(250);

        builder.Property(o => o.AddressLine2)
            .HasMaxLength(250);

        builder.Property(o => o.City)
            .HasMaxLength(100);

        builder.Property(o => o.State)
            .HasMaxLength(100);

        builder.Property(o => o.Country)
            .HasMaxLength(100);

        builder.Property(o => o.PostalCode)
            .HasMaxLength(20);

        builder.Property(o => o.Website)
            .HasMaxLength(256);

        builder.Property(o => o.Email)
            .HasMaxLength(256);

        builder.Property(o => o.PhoneNumber)
            .HasMaxLength(50);

        builder.Property(o => o.AnnualBudget)
            .HasColumnType("decimal(18,2)");

        builder.Property(o => o.NumberOfEmployees);

        builder.Property(o => o.PrimaryContactName)
            .HasMaxLength(200);

        builder.Property(o => o.PrimaryContactEmail)
            .HasMaxLength(256);

        builder.Property(o => o.PrimaryContactPhone)
            .HasMaxLength(50);

        builder.Property(o => o.Description)
            .HasMaxLength(2000);

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.Property(o => o.CreatedBy)
            .HasMaxLength(200);

        builder.Property(o => o.UpdatedBy)
            .HasMaxLength(200);

        builder.HasQueryFilter(o => !o.IsDeleted);
    }
}

