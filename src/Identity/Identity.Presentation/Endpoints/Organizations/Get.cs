using FastEndpoints;
using Identity.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Identity.Presentation.Endpoints.Organizations;

public record GetOrganizationRequest
{
    public Guid Id { get; set; }
}

public class GetOrganizationResponse
{
    public Guid Id { get; set; }
    public string LegalName { get; set; } = null!;
    public string? TradeName { get; set; }
    public string TaxId { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string ContactEmail { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}

public class GetOrganizationEndpoint(IdentityDbContext context)
    : Endpoint<GetOrganizationRequest, GetOrganizationResponse>
{
    public override void Configure()
    {
        Get("/api/v1/organizations/{Id}");
        AllowAnonymous();
        Summary(s => {
            s.Summary = "Get organization by ID";
            s.Description = "Retrieves the details of an organization.";
        });
    }

    public override async Task HandleAsync(GetOrganizationRequest req, CancellationToken ct)
    {
        var dataModel = await context.Organizations
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == req.Id, ct);

        if (dataModel is null)
        {
            await Send.ResultAsync(TypedResults.NotFound());
            return;
        }

        var response = new GetOrganizationResponse
        {
            Id = dataModel.Id,
            LegalName = dataModel.LegalName,
            TradeName = dataModel.TradeName,
            TaxId = dataModel.TaxId,
            Country = dataModel.Country,
            ContactEmail = dataModel.ContactEmail,
            CreatedAt = dataModel.CreatedAt
        };

        await Send.ResultAsync(TypedResults.Ok(response));
    }
}
