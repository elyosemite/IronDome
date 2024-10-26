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
        var meter = new Meter("CustomMetrics", "1.0");
        builder.Services.AddSingleton(meter);

        builder.Services
            .AddOpenTelemetry()
            .WithMetrics(metric =>
            {
                metric
                    .AddPrometheusExporter()
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddMeter("CustomMetrics");
            });

        return builder;
    }
}