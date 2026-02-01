using FastEndpoints;
using FastEndpoints.Swagger;
using Identity.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

builder.Services.AddDbContext<IdentityDbContext>((sp, options) =>
{
    var logger = sp.GetRequiredService<ILogger<Program>>();
    var env = sp.GetRequiredService<IHostEnvironment>();
    logger.LogInformation("Configuring IdentityDbContext with PostgreSQL. Environment: {EnvironmentName}", env.EnvironmentName);
    logger.LogWarning("Sensitive data logging is enabled");
    
    options.UseNpgsql(connectionString)
           .EnableSensitiveDataLogging()
           .EnableDetailedErrors();
});

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

var app = builder.Build();

app.UseFastEndpoints();
app.UseSwaggerGen();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<IdentityDbContext>();
        // Using MigrateAsync to apply migrations
        await context.Database.MigrateAsync(); 
        logger.LogInformation("Database created/migrated successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while creating/migrating the database.");
        // Re-throw if you want startup to fail
        throw; 
    }
}

app.Run();
