using CodeCrafters.Domain.Entities.Organisations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeCrafters.Infrastructure.Data.Configurations;

public class OrganisationConfiguration : IEntityTypeConfiguration<Organisation>
{
    public void Configure(EntityTypeBuilder<Organisation> builder)
    {
        builder.ToTable("Organisations");
        builder.HasKey(o => o.Id);

        builder.HasIndex(o => o.UserId).IsUnique();

        builder.Property(o => o.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(o => o.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(o => o.Type)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(o => o.State)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(o => o.AnnualBudget)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(o => o.ContactPersonName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(o => o.ContactPersonEmail)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(o => o.ContactPersonPhone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(o => o.YearOfEstablishment)
            .IsRequired();

        builder.Property(o => o.IsProfileComplete)
            .HasDefaultValue(false);

        builder.HasOne(o => o.User)
            .WithOne()
            .HasForeignKey<Organisation>(o => o.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
