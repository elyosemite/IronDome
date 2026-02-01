using FastEndpoints;
using Identity.Domain.Enums;
using Identity.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Identity.Presentation.Endpoints.Subjects;

public class GetSubjectRequest
{
    public Guid Id { get; set; }
}

public class GetSubjectResponse
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public string OrganizationName { get; set; } = null!;
    public string CommonName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public SubjectType Type { get; set; }
    public string? Department { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetSubjectEndpoint : Endpoint<GetSubjectRequest, GetSubjectResponse>
{
    private readonly IdentityDbContext _context;

    public GetSubjectEndpoint(IdentityDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/api/v1/subjects/{Id}");
        AllowAnonymous();
        Summary(s => {
            s.Summary = "Get subject by ID";
            s.Description = "Retrieves the details of a subject including organization summary.";
        });
    }

    public override async Task HandleAsync(GetSubjectRequest req, CancellationToken ct)
    {
        var dataModel = await _context.Subjects
            .AsNoTracking()
            .Include(s => s.Organization)
            .FirstOrDefaultAsync(s => s.Id == req.Id, ct);

        if (dataModel is null)
        {
            await Send.ResultAsync(TypedResults.NotFound());
            return;
        }

        var response = new GetSubjectResponse
        {
            Id = dataModel.Id,
            OrganizationId = dataModel.OrganizationId,
            OrganizationName = dataModel.Organization.LegalName,
            CommonName = dataModel.CommonName,
            Email = dataModel.Email,
            Type = dataModel.Type,
            Department = dataModel.Department,
            CreatedAt = dataModel.CreatedAt
        };

        await Send.ResultAsync(TypedResults.Ok(response));
    }
}
