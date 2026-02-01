using FastEndpoints;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Identity.Infrastructure;
using Identity.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Identity.Presentation.Endpoints.Subjects;

public class CreateSubjectRequest
{
    public Guid OrganizationId { get; set; }
    public string CommonName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public SubjectType Type { get; set; }
    public string? Department { get; set; }
}

public class CreateSubjectResponse
{
    public Guid Id { get; set; }
    public Dictionary<string, string> DistinguishedNameAttributes { get; set; } = null!;
}

public class CreateSubjectEndpoint : Endpoint<CreateSubjectRequest, CreateSubjectResponse>
{
    private readonly IdentityDbContext _context;

    public CreateSubjectEndpoint(IdentityDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Post("/api/v1/organizations/{OrganizationId}/subjects");
        AllowAnonymous();
        Summary(s => {
            s.Summary = "Create a new subject";
            s.Description = "Registers a new actor (Person, System, Device) linked to an organization.";
        });
    }

    public override async Task HandleAsync(CreateSubjectRequest req, CancellationToken ct)
    {
        // 1. Verify Organization exists
        var org = await _context.Organizations
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == req.OrganizationId, ct);

        if (org is null)
        {
            await Send.ResultAsync(TypedResults.NotFound());
            return;
        }

        // 2. Create Domain Entity
        var subject = new Subject(
            req.OrganizationId,
            req.CommonName,
            req.Email,
            req.Type,
            req.Department
        );

        // 3. Map to Data Model
        var dataModel = SubjectDataModel.FromDomain(subject);

        // 4. Persist
        _context.Subjects.Add(dataModel);
        await _context.SaveChangesAsync(ct);

        // 5. Compute DN attributes for response (as per Identity_API.md)
        var dnAttributes = new Dictionary<string, string>
        {
            { "CN", subject.CommonName },
            { "O", org.LegalName },
            { "C", org.Country }
        };
        
        if (!string.IsNullOrEmpty(subject.Department))
        {
            dnAttributes.Add("OU", subject.Department);
        }

        // 6. Respond
        await Send.ResultAsync(TypedResults.Created($"/api/v1/subjects/{subject.Id}", new CreateSubjectResponse
        {
            Id = subject.Id,
            DistinguishedNameAttributes = dnAttributes
        }));
    }
}
