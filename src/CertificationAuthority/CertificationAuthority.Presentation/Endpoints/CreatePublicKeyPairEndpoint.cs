using CertificationAuthority.Application.UseCases.CreatePublicPrivateKeyPair;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace CertificationAuthority.Presentation.Endpoints;

public static class CreatePublicKeyPairEndpoint
{
    public static async Task<IResult> ExecuteAsync(IMediator mediator, [FromServices] ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("GenericLogger");
        var request = new CreatePublicPrivateKeyPairRequest();
        CreatePublicPrivateKeyPairResponse response = await mediator.Send(request);

        logger.LogInformation("Generating Public and Private Keys. Public Key = {PublicKey}", response.PublicKey);

        using (var memoryStream = new MemoryStream())
        {
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var publicKeyEntry = zipArchive.CreateEntry("public_key.pem");
                using (var publicKeyStream = publicKeyEntry.Open())
                {
                    await publicKeyStream.WriteAsync(response.PublicKey, 0, response.PublicKey.Length);
                }

                var privateKeyEntry = zipArchive.CreateEntry("private_key.der");
                using (var privateKeyStream = privateKeyEntry.Open())
                {
                    await privateKeyStream.WriteAsync(response.PrivateKey, 0, response.PrivateKey.Length);
                }
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            return Results.File(memoryStream.ToArray(), "application/zip", "keys.zip");
        }
    }
}
