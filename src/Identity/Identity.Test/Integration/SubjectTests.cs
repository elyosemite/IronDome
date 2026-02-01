using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Identity.Domain.Enums;
using Identity.Presentation.Endpoints.Organizations;
using Identity.Presentation.Endpoints.Subjects;
using NUnit.Framework;

namespace Identity.Test.Integration;

[TestFixture]
public class SubjectTests : IntegrationTestBase
{
    private Guid _organizationId;

    [OneTimeSetUp]
    public async Task SetupOrganization()
    {
        // Ensure base setup is done (NUnit does this automatically for OneTimeSetUp if base has it, 
        // but since base also has OneTimeSetUp, they both run. 
        // However, Client property is set in base.
        
        // Wait for base to initialize? NUnit guarantees base OneTimeSetUp runs before derived.
        
        // Create an organization for subject tests
        var orgRequest = new CreateOrganizationRequest
        {
            LegalName = "Subject Test Org",
            TaxId = "11223344",
            Country = "BRA",
            ContactEmail = "org@test.com"
        };
        var response = await Client.PostAsJsonAsync("/api/v1/organizations", orgRequest);
        var content = await response.Content.ReadFromJsonAsync<CreateOrganizationResponse>();
        _organizationId = content!.Id;
    }

    [Test]
    public async Task CreateSubject_ShouldReturnCreated()
    {
        // Arrange
        var request = new CreateSubjectRequest
        {
            OrganizationId = _organizationId,
            CommonName = "Test Subject",
            Email = $"subject_{Guid.NewGuid()}@test.com",
            Type = SubjectType.Person,
            Department = "Engineering"
        };

        // Act
        var response = await Client.PostAsJsonAsync($"/api/v1/organizations/{_organizationId}/subjects", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var content = await response.Content.ReadFromJsonAsync<CreateSubjectResponse>();
        content.Should().NotBeNull();
        content!.DistinguishedNameAttributes.Should().ContainKey("CN");
        content.DistinguishedNameAttributes["CN"].Should().Be(request.CommonName);
    }

    [Test]
    public async Task GetSubject_ShouldReturnDetails()
    {
        // Arrange - Create first
        var createRequest = new CreateSubjectRequest
        {
            OrganizationId = _organizationId,
            CommonName = "Get Details Subject",
            Email = $"get_{Guid.NewGuid()}@test.com",
            Type = SubjectType.System
        };
        var createResponse = await Client.PostAsJsonAsync($"/api/v1/organizations/{_organizationId}/subjects", createRequest);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateSubjectResponse>();

        // Act
        var response = await Client.GetAsync($"/api/v1/subjects/{created!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<GetSubjectResponse>();
        content.Should().NotBeNull();
        content!.CommonName.Should().Be(createRequest.CommonName);
        content.OrganizationName.Should().Be("Subject Test Org");
    }

    [Test]
    public async Task SearchSubjects_ByEmail_ShouldReturnMatch()
    {
        // Arrange
        var email = $"search_{Guid.NewGuid()}@test.com";
        var createRequest = new CreateSubjectRequest
        {
            OrganizationId = _organizationId,
            CommonName = "Search Me",
            Email = email,
            Type = SubjectType.Device
        };
        await Client.PostAsJsonAsync($"/api/v1/organizations/{_organizationId}/subjects", createRequest);

        // Act
        var response = await Client.GetAsync($"/api/v1/subjects?email={email}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var results = await response.Content.ReadFromJsonAsync<List<SubjectSummaryResponse>>();
        results.Should().NotBeNull();
        results.Should().ContainSingle(s => s.Email == email);
    }
}
