using Identity.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Configurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<OrganizationDataModel>
{
    public void Configure(EntityTypeBuilder<OrganizationDataModel> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.LegalName)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(o => o.TradeName)
            .HasMaxLength(200);

        builder.Property(o => o.TaxId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(o => o.Country)
            .IsRequired()
            .HasMaxLength(3); // ISO Alpha-3

        builder.Property(o => o.ContactEmail)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(a => a.Address_Street).HasMaxLength(200);
        builder.Property(a => a.Address_Number).HasMaxLength(20);
        builder.Property(a => a.Address_City).HasMaxLength(100);
        builder.Property(a => a.Address_State).HasMaxLength(50);
        builder.Property(a => a.Address_ZipCode).HasMaxLength(20);
        
        builder.Property(o => o.CreatedAt)
            .IsRequired();
    }
}