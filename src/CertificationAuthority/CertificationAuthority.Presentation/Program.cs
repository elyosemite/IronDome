using CertificationAuthority.Application;
using CertificationAuthority.Presentation.Endpoints;
using Serilog;
using Observability;
using CertificationAuthority.Presentation;

var builder = WebApplication.CreateBuilder(args);

ThreadPool.SetMinThreads(100, 100);

// Configuration of Tracing, Logging (with Serilog) and Metrics
builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder
    .Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true);

builder.Host.UseSerilog();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("QA") || app.Environment.IsEnvironment("Homolog"))
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", $"My API V1 - {app.Environment.EnvironmentName}");
        c.RoutePrefix = string.Empty;
    });
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseMiddleware<RequestCounterMiddleware>();
app.MapPrometheusScrapingEndpoint();

app.MapGet("/key-pair", CreatePublicKeyPairEndpoint.ExecuteAsync)
    .WithName("KeyPair")
    .WithOpenApi();

app.MapGet("/certificate", CreateCertificateEndpoint.ExecuteAsync)
    .WithName("Certificate")
    .WithOpenApi();

try
{
    Log.Information($"Starting web host on {app.Environment.EnvironmentName} Environment");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}