using CodeCrafters.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeCrafters.Infrastructure.Data.Configurations;

public class UserDocumentConfiguration : IEntityTypeConfiguration<UserDocument>
{
    public void Configure(EntityTypeBuilder<UserDocument> builder)
    {
        builder.ToTable("UserDocuments");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.DocumentType).IsRequired().HasMaxLength(50);
        builder.Property(d => d.FileName).IsRequired().HasMaxLength(255);
        builder.Property(d => d.ContentType).IsRequired().HasMaxLength(100);
        builder.Property(d => d.StoredPath).IsRequired().HasMaxLength(500);
        builder.HasIndex(d => d.UserId);
        builder.HasOne(d => d.User)
            .WithMany()
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
