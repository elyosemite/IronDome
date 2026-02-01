using Identity.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Configurations;

public class SubjectConfiguration : IEntityTypeConfiguration<SubjectDataModel>
{
    public void Configure(EntityTypeBuilder<SubjectDataModel> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.CommonName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(s => s.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(s => s.Department)
            .HasMaxLength(100);

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        // Foreign Key
        builder.HasOne(s => s.Organization)
            .WithMany()
            .HasForeignKey(s => s.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique Email per Organization
        builder.HasIndex(s => new { s.OrganizationId, s.Email })
            .IsUnique();
    }
}
