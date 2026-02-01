using System;
using Identity.Domain.Entities;

namespace Identity.Infrastructure.Models;

public class OrganizationDataModel
{
    public Guid Id { get; set; }
    public string LegalName { get; set; } = null!;
    public string? TradeName { get; set; }
    public string TaxId { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string ContactEmail { get; set; } = null!;
    public string? Address_Street { get; set; }
    public string? Address_Number { get; set; }
    public string? Address_City { get; set; }
    public string? Address_State { get; set; }
    public string? Address_ZipCode { get; set; }
    public DateTime CreatedAt { get; set; }

    public static OrganizationDataModel FromDomain(Organization organization)
    {
        return new OrganizationDataModel
        {
            Id = organization.Id,
            LegalName = organization.LegalName,
            TradeName = organization.TradeName,
            TaxId = organization.TaxId,
            Country = organization.Country,
            ContactEmail = organization.ContactEmail,
            Address_Street = organization.Address?.Street,
            Address_Number = organization.Address?.Number,
            Address_City = organization.Address?.City,
            Address_State = organization.Address?.State,
            Address_ZipCode = organization.Address?.ZipCode,
            CreatedAt = organization.CreatedAt
        };
    }
}
