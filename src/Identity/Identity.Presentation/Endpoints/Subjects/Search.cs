using Ardalis.Specification.EntityFrameworkCore;
using FastEndpoints;
using Identity.Domain.Enums;
using Identity.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Identity.Presentation.Endpoints.Subjects;

public class SearchSubjectsRequest
{
    public string? Email { get; set; }
    public Guid? OrganizationId { get; set; }
}

public class SubjectSummaryResponse
{
    public Guid Id { get; set; }
    public string CommonName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public SubjectType Type { get; set; }
}

public class SearchSubjectsEndpoint : Endpoint<SearchSubjectsRequest, List<SubjectSummaryResponse>>
{
    private readonly IdentityDbContext _context;

    public SearchSubjectsEndpoint(IdentityDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/api/v1/subjects");
        AllowAnonymous();
        Summary(s => {
            s.Summary = "Search for subjects";
            s.Description = "Filters subjects by email or organization.";
        });
    }

    public override async Task HandleAsync(SearchSubjectsRequest req, CancellationToken ct)
    {
        var query = _context.Subjects.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(req.Email))
        {
            query = query.Where(s => s.Email == req.Email);
        }

        if (req.OrganizationId.HasValue && req.OrganizationId != Guid.Empty)
        {
            query = query.Where(s => s.OrganizationId == req.OrganizationId.Value);
        }

        var results = await query
            .Select(s => new SubjectSummaryResponse
            {
                Id = s.Id,
                CommonName = s.CommonName,
                Email = s.Email,
                Type = s.Type
            })
            .ToListAsync(ct);

        await Send.ResultAsync(TypedResults.Ok(results));
    }
}
