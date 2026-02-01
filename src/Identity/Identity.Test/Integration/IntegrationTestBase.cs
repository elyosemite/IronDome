using Identity.Presentation.Endpoints.Organizations; // Add using
using Identity.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Testcontainers.PostgreSql;

namespace Identity.Test.Integration;

public class IntegrationTestBase
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .Build();

    protected WebApplicationFactory<CreateOrganizationEndpoint> Factory { get; private set; } // Change type
    protected HttpClient Client { get; private set; }

    [OneTimeSetUp]
    public async Task GlobalSetup()
    {
        await _dbContainer.StartAsync();

        Factory = new WebApplicationFactory<CreateOrganizationEndpoint>() // Change type
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove existing DbContext registration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<IdentityDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add DbContext with container connection string
                    services.AddDbContext<IdentityDbContext>(options =>
                    {
                        options.UseNpgsql(_dbContainer.GetConnectionString());
                    });
                });
            });

        Client = Factory.CreateClient();

        // Ensure database is created
        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
    }

    [OneTimeTearDown]
    public async Task GlobalTearDown()
    {
        Client.Dispose();
        await Factory.DisposeAsync();
        await _dbContainer.DisposeAsync();
    }
}
