using System;
using Identity.Domain.Entities;
using Identity.Domain.Enums;

namespace Identity.Infrastructure.Models;

public class SubjectDataModel
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string CommonName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public SubjectType Type { get; set; }
    public string? Department { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation property
    public OrganizationDataModel Organization { get; set; } = null!;

    public static SubjectDataModel FromDomain(Subject subject)
    {
        return new SubjectDataModel
        {
            Id = subject.Id,
            OrganizationId = subject.OrganizationId,
            CommonName = subject.CommonName,
            Email = subject.Email,
            Type = subject.Type,
            Department = subject.Department,
            CreatedAt = subject.CreatedAt
        };
    }
}
