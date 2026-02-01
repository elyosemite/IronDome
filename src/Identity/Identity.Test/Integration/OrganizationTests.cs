using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Identity.Presentation.Endpoints.Organizations;
using NUnit.Framework;

namespace Identity.Test.Integration;

[TestFixture]
public class OrganizationTests : IntegrationTestBase
{
    [Test]
    public async Task CreateOrganization_ShouldReturnCreated()
    {
        // Arrange
        var request = new CreateOrganizationRequest
        {
            LegalName = "Integration Test Corp",
            TaxId = "1234567890",
            Country = "USA",
            ContactEmail = "test@integration.com"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/v1/organizations", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var content = await response.Content.ReadFromJsonAsync<CreateOrganizationResponse>();
        content.Should().NotBeNull();
        content!.Id.Should().NotBeEmpty();
    }

    [Test]
    public async Task GetOrganization_ShouldReturnDetails()
    {
        // Arrange - Create first
        var createRequest = new CreateOrganizationRequest
        {
            LegalName = "Get Test Corp",
            TaxId = "987654321",
            Country = "BR",
            ContactEmail = "get@test.com"
        };
        var createResponse = await Client.PostAsJsonAsync("/api/v1/organizations", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateOrganizationResponse>();

        // Act
        var response = await Client.GetAsync($"/api/v1/organizations/{created!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<GetOrganizationResponse>();
        content.Should().NotBeNull();
        content!.LegalName.Should().Be(createRequest.LegalName);
    }
}