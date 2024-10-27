using Microsoft.Extensions.Hosting;

namespace Observability;

public static class CollectionExtensions
{
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        builder
            .AddTracing()
            .AddMetrics()
            .AddLogging();

        return builder;
    }
}