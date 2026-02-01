using System;
using Ardalis.SharedKernel;
using Identity.Domain.ValueObjects;

namespace Identity.Domain.Entities;

public class Organization : EntityBase<Guid>, IAggregateRoot
{
    public string LegalName { get; private set; }
    public string? TradeName { get; private set; }
    public string TaxId { get; private set; }
    public string Country { get; private set; }
    public string ContactEmail { get; private set; }
    public Address? Address { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // EF Core constructor
    private Organization() { }

    public Organization(string legalName, string taxId, string country, string contactEmail, Address? address = null, string? tradeName = null)
    {
        if (string.IsNullOrWhiteSpace(legalName))
            throw new ArgumentException("Legal Name is required", nameof(legalName));

        if (string.IsNullOrWhiteSpace(taxId))
            throw new ArgumentException("Tax ID is required", nameof(taxId));
        
        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country is required", nameof(country));
        
        if (string.IsNullOrWhiteSpace(contactEmail))
            throw new ArgumentException("Contact Email is required", nameof(contactEmail));

        Id = Guid.NewGuid();
        LegalName = legalName;
        TaxId = taxId;
        Country = country;
        ContactEmail = contactEmail;
        Address = address;
        TradeName = tradeName;
        CreatedAt = DateTime.UtcNow;
    }
}
