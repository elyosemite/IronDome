using CertificationAuthority.Domain.Builders;
using CertificationAuthority.Domain.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace CertificationAuthority.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
        services.AddScoped<ICertificateBuilder, CertificateBuilder>();
        services.AddScoped<ICertificateFactory, PKICertificateFactory>();
        return services;
    }
}
