using CertificationAuthority.Domain.Builders;
using CertificationAuthority.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CertificationAuthority.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Registrar o MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

        // Registrar o gerador de par de chaves RSA
        services.AddScoped<IPublicKeyPairGenerator, PublicKeyPairGenerator>();
        services.AddScoped<ICertificateBuilder, CertificateBuilder>();
        services.AddScoped<ICertificateGenerator, CertificateGenerator>();

        return services;
    }
}
