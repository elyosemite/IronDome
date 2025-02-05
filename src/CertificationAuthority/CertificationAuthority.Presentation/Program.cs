using CertificationAuthority.Application;
using CertificationAuthority.Presentation.Endpoints;
using SharedKernel.Settings;
using Serilog;
using Observability;
using CertificationAuthority.Presentation;
using OpenTelemetry;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

var tracerProvider = Sdk.CreateTracerProviderBuilder()
    .AddSource("CertificationAuthority.CertificationAuthority.Presentation")
    //.AddConsoleExporter()
    .AddJaegerExporter()
    .Build();

ThreadPool.SetMinThreads(100, 100);

// Configuration of Tracing, Logging (with Serilog) and Metrics
builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddLogging();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

// Configure Option Pattern for GlobalSettings
builder.Services.Configure<GlobalSettings>(builder.Configuration.GetSection("GlobalSettings"));

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

app.MapGet("/guid", GetGuidEndpoint.ExecuteAsync)
    .WithName("Guid")
    .WithOpenApi();

try
{
    var urls = string.Join(", ", app.Urls);
    Log.Information("Starting web host on {EnvironmentName} Environment created by {Author}. The ApiKey is {ApiKey}. Listening on {Urls}", app.Environment.EnvironmentName, builder.Configuration["GlobalSettings:Author"], builder.Configuration["GlobalSettings:ApiKey"], urls);
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}