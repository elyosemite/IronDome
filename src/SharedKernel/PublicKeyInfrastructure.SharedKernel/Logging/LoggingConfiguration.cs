using Microsoft.Extensions.Configuration;
using Serilog;

namespace PublicKeyInfrastructure.SharedKernel.Logging;

public static class LoggingConfiguration
{
    public static void Logging(IConfiguration configuration)
    {
        var seqURL = configuration["Serilog:SeqUrl"] ?? string.Empty;

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithProcessId()
            .Enrich.WithEnvironmentName()
            .WriteTo.Console()
            .WriteTo.Seq(seqURL)
            .CreateLogger();
    }
}
