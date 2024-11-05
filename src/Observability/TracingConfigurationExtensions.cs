using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Trace;

namespace Observability;

public static class TracingConfigurationExtensions
{
    public static IHostApplicationBuilder AddTracing(this IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton(TracerProvider.Default);

        builder.Services
            .AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation()
                    .AddSource("CustomSource")
                    .SetSampler(new AlwaysOnSampler())
                    .AddJaegerExporter(options =>
                    {
                        options.AgentHost = "localhost";
                        options.AgentPort = 6831;
                    });
            });

        return builder;
    }
}