using System.IO.Compression;
using CertificationAuthority.Application;
using CertificationAuthority.Application.UseCases.CreatePublicPrivateKeyPair;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/key-pair", async (IMediator mediator) =>
{
    var request = new CreatePublicPrivateKeyPairRequest();
    CreatePublicPrivateKeyPairResponse response = await mediator.Send(request);

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
})
.WithName("KeyPair")
.WithOpenApi();

app.Run();