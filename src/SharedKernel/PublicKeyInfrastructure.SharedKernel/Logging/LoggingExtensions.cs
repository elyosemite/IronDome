using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace PublicKeyInfrastructure.SharedKernel.Logging;

public static class LoggingExtensions
{
    public static void AddLoggingConfiguration(this IServiceCollection builder, IConfiguration configuration)
    {
        builder.AddLogging();
        LoggingConfiguration.Logging(configuration);
    }

    public static void UseSerilogWithConfiguration(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog();
    }
}
