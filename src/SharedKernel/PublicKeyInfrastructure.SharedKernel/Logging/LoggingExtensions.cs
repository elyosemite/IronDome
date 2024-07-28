using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace PublicKeyInfrastructure.SharedKernel.Logging;

public static class LoggingExtensions
{
    public static void AddLoggingConfiguration(this IServiceCollection builder, IConfigurationBuilder configurationBuilder)
    {
        builder.AddLogging();
        configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        LoggingConfiguration.Logging(configurationBuilder.Build());
    }

    public static void UseSerilogWithConfiguration(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog();
    }
}
