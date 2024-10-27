using System.Diagnostics.Metrics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Observability;

public static class MetricsConfigurationExtensions
{
    public static IHostApplicationBuilder AddMetrics(this IHostApplicationBuilder builder)
    {
        //var meter = new Meter("CustomMetrics123", "1.0");
        //builder.Services.AddSingleton(meter);

        builder.Services
            .AddOpenTelemetry()
            .WithMetrics(metric =>
            {
                metric
                    .AddConsoleExporter()
                    .AddPrometheusExporter()
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddMeter(MetricsConfig.MetricName);
            });

        return builder;
    }
}