using CodeCrafters.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeCrafters.Infrastructure.Data.Configurations;

public class DocumentVaultItemConfiguration : IEntityTypeConfiguration<DocumentVaultItem>
{
    public void Configure(EntityTypeBuilder<DocumentVaultItem> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.DocumentType).IsRequired().HasMaxLength(100);
        builder.Property(d => d.OriginalFileName).IsRequired().HasMaxLength(500);
        builder.Property(d => d.StoredFilePath).IsRequired().HasMaxLength(1000);
        builder.Property(d => d.ContentType).IsRequired().HasMaxLength(200);

        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(d => new { d.UserId, d.DocumentType });
    }
}
