using System.IO.Compression;
using MediatR;
using CertificationAuthority.Application.UseCases.CreateCertificate;
using CertificationAuthority.Application.UseCases.CreatePublicPrivateKeyPair;
using CertificationAuthority.Domain.Enumerations;
using System.Diagnostics;
using Observability;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace CertificationAuthority.Presentation.Endpoints;

public static class CreateCertificateEndpoint
{
    public static async Task<IResult> ExecuteAsync(IMediator mediator, [FromServices] ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("GenericLogger");

        var ironDomeRootIssuerPublicKeyPair = await mediator.Send(new CreatePublicPrivateKeyPairRequest());
        logger.LogInformation("Iron Dome Root CA's Generated Key");

        var subjectPublicKeyPair = await mediator.Send(new CreatePublicPrivateKeyPairRequest());
        logger.LogInformation("Subject's Generated Key");

        var request = new CreateCertificateRequest(
            "Iron Dome DN",
            "203948239084",
            DateTime.Parse("2021/10/13 12:00:00", new CultureInfo("pt-BR")),
            DateTime.Parse("2030/10/1 00:00:00", new CultureInfo("pt-BR")),
            "Yuri Melo",
            Convert.ToBase64String(subjectPublicKeyPair.PublicKey),
            Convert.ToBase64String(ironDomeRootIssuerPublicKeyPair.PrivateKey),
            SignatureAlgorithmEnum.SHA256WithRSA);

        var response = await mediator.Send(request);

        using (var memoryStream = new MemoryStream())
        {
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var certificate = zipArchive.CreateEntry("certificate.der");
                using (var certificateStream = certificate.Open())
                {
                    using (Activity? activity = ActivityTracing.Source.StartActivity("CreateCertificate"))
                    {
                        await certificateStream.WriteAsync(response.Certificate, 0, response.Certificate.Length);
                    }
                }
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            return Results.File(memoryStream.ToArray(), "application/zip", "certificate.zip");
        }
    }
}
