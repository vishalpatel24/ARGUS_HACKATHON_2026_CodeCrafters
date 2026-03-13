using CodeCrafters.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeCrafters.Infrastructure.Data.Configurations;

public class ScreeningCheckConfiguration : IEntityTypeConfiguration<ScreeningCheck>
{
    public void Configure(EntityTypeBuilder<ScreeningCheck> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.CheckCode).IsRequired().HasMaxLength(50);
        builder.Property(c => c.CheckName).IsRequired().HasMaxLength(200);
        builder.Property(c => c.CheckType).IsRequired().HasMaxLength(20);
        builder.Property(c => c.Result).IsRequired().HasMaxLength(20);
        builder.Property(c => c.Details).HasMaxLength(2000);
    }
}
