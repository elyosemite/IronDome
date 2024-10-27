using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Logs;
using Serilog.Sinks.Grafana.Loki;
using Serilog;

namespace Observability;

public static class LoggingConfigurationExtensions
{
    public static IHostApplicationBuilder AddLogging(this IHostApplicationBuilder builder)
    {
        builder.Services
            .AddOpenTelemetry()
            .WithLogging(logging =>
            {
                logging.AddConsoleExporter();
            });

        var seqURL = builder.Configuration["Serilog:SeqUrl"] ?? string.Empty;

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithProcessId()
            .Enrich.WithEnvironmentName()
            .WriteTo.Console()
            .WriteTo.Seq(seqURL)
            .WriteTo.GrafanaLoki("http://host.docker.internal:3100", labels: new[] { new LokiLabel { Key = "app", Value = "IronDome" } })
            .CreateLogger();
        
        return builder;
    }
}