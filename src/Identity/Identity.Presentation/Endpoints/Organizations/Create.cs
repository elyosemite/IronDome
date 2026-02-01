using FastEndpoints;
using Identity.Domain.Entities;
using Identity.Infrastructure;
using Identity.Infrastructure.Models;

namespace Identity.Presentation.Endpoints.Organizations;

public class CreateOrganizationRequest
{
    public string LegalName { get; set; } = null!;
    public string? TradeName { get; set; }
    public string TaxId { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string ContactEmail { get; set; } = null!;
}

public class CreateOrganizationResponse
{
    public Guid Id { get; set; }
}

public class CreateOrganizationEndpoint(IdentityDbContext context)
    : Endpoint<CreateOrganizationRequest, CreateOrganizationResponse>
{
    public override void Configure()
    {
        Post("/api/v1/organizations");
        AllowAnonymous();
        Summary(s => {
            s.Summary = "Create a new organization";
            s.Description = "Registers a new legal entity in the Identity Service.";
        });
    }

    public override async Task HandleAsync(CreateOrganizationRequest req, CancellationToken ct)
    {
        // 1. Create Domain Entity
        var organization = new Organization(
            req.LegalName,
            req.TaxId,
            req.Country,
            req.ContactEmail,
            tradeName: req.TradeName
        );

        // 2. Map to Data Model
        var dataModel = OrganizationDataModel.FromDomain(organization);

        // 3. Persist
        context.Organizations.Add(dataModel);
        await context.SaveChangesAsync(ct);

        // 4. Respond
        await Send.ResultAsync(TypedResults.Created($"/api/v1/organizations/{organization.Id}", new CreateOrganizationResponse
        {
            Id = organization.Id
        }));
    }
}