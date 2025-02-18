using System.IO.Compression;
using MediatR;
using CertificationAuthority.Application.UseCases.CreatePublicPrivateKeyPair;
using CertificationAuthority.Domain.Enumerations;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using CertificationAuthority.Application.UseCases.CreateCsrFile;
using CertificationAuthority.Infrastructure.Extensions;

namespace CertificationAuthority.Presentation.Endpoints;

public static class CreateCsrFilepoint
{
    public static async Task<IResult> ExecuteAsync(IMediator mediator, [FromServices] ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("GenericLogger");

        var subjectPublicKeyPair = await mediator.Send(new CreatePublicPrivateKeyPairRequest());
        logger.LogInformation("Subject's Generated Key");
        var csrFileName = "certificate.csr";
        var privateKeyFileName = "privateKey.pfx";

        var request = new CreateCsrFileRequest(
            "CloudBeholder",
            Convert.ToBase64String(subjectPublicKeyPair.PublicKey),
            Encoding.UTF8.GetString(KeyExtension.ConvertToPem(KeyExtension.ParseFromPEMPrivateKey(Encoding.UTF8.GetString(subjectPublicKeyPair.PrivateKey)))),
            SignatureAlgorithmEnum.SHA256WithRSA.Name,
            csrFileName,
            privateKeyFileName);

        var response = await mediator.Send(request);

        var zipFileName = "certificate_bundle.zip";

        using (var memoryStream = new MemoryStream())
        {
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var csrEntry = archive.CreateEntry(csrFileName);
                using (var entryStream = csrEntry.Open())
                using (var writer = new StreamWriter(entryStream))
                {
                    await writer.WriteAsync(Encoding.UTF8.GetString(response.CsrFile));
                }

                var privateKeyEntry = archive.CreateEntry(privateKeyFileName);
                using (var entryStream = privateKeyEntry.Open())
                using (var writer = new StreamWriter(entryStream))
                {
                    await writer.WriteAsync(Encoding.UTF8.GetString(response.PrivateKey));
                }
            }

            return Results.File(memoryStream.ToArray(), "application/zip", zipFileName);
        }
    }
}
