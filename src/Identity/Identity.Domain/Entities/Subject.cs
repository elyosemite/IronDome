using Ardalis.SharedKernel;
using Identity.Domain.Enums;

namespace Identity.Domain.Entities;

public class Subject : EntityBase<Guid>, IAggregateRoot
{
    public Guid OrganizationId { get; private set; }
    public string CommonName { get; private set; }
    public string Email { get; private set; }
    public SubjectType Type { get; private set; }
    public string? Department { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // EF Core constructor
    private Subject() { }

    public Subject(Guid organizationId, string commonName, string email, SubjectType type, string? department = null)
    {
        if (organizationId == Guid.Empty)
            throw new ArgumentException("OrganizationId is required", nameof(organizationId));

        if (string.IsNullOrWhiteSpace(commonName))
            throw new ArgumentException("Common Name is required", nameof(commonName));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required", nameof(email));

        Id = Guid.NewGuid();
        OrganizationId = organizationId;
        CommonName = commonName;
        Email = email;
        Type = type;
        Department = department;
        CreatedAt = DateTime.UtcNow;
    }
}
