using System.IO.Compression;
using MediatR;
using CertificationAuthority.Application.UseCases.CreateCertificate;
using CertificationAuthority.Application.UseCases.CreatePublicPrivateKeyPair;
using CertificationAuthority.Domain.Enumerations;
using System.Text;

namespace CertificationAuthority.Presentation.Endpoints;

public class CreateCertificateEndpoint
{
    public static async Task<IResult> ExecuteAsync(IMediator mediator)
    {
        var ironDomeRootIssuerPublicKeyPair = await mediator.Send(new CreatePublicPrivateKeyPairRequest());
        var subjectPublicKeyPair = await mediator.Send(new CreatePublicPrivateKeyPairRequest());

        var request = new CreateCertificateRequest(
            "Iron Dome DN",
            "203948239084",
            DateTime.Parse("2021/10/13 12:00:00"),
            DateTime.Parse("2024/10/1 00:00:00"),
            "Yuri Melo",
            Encoding.ASCII.GetString(ironDomeRootIssuerPublicKeyPair.PublicKey),
            Encoding.ASCII.GetString(subjectPublicKeyPair.PrivateKey),
            SignatureAlgorithmEnum.SHA256WithRSA);

        var response = await mediator.Send(request);

        using (var memoryStream = new MemoryStream())
        {
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var certificate = zipArchive.CreateEntry("certificate.pem");
                using (var publicKeyStream = certificate.Open())
                {
                    await publicKeyStream.WriteAsync(response.Certificate, 0, response.Certificate.Length);
                }
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            return Results.File(memoryStream.ToArray(), "application/zip", "certificate.zip");
        }
    }
}
