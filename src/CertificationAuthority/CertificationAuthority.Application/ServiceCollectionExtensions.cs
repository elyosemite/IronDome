using CertificationAuthority.Domain.Builders;
using CertificationAuthority.Domain.Factories;
using CertificationAuthority.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CertificationAuthority.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

        services.AddScoped<IPublicKeyPairGenerator, PublicKeyPairGenerator>();
        services.AddScoped<ICertificateBuilder, CertificateBuilder>();
        services.AddScoped<ICertificateGenerator, CertificateGenerator>();
        services.AddScoped<ICertificateFactory, PKICertificateFactory>();

        return services;
    }
}
