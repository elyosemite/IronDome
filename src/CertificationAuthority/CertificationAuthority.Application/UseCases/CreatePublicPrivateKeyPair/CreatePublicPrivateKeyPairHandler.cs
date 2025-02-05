using CertificationAuthority.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CertificationAuthority.Application.UseCases.CreatePublicPrivateKeyPair;

public class CreatePublicPrivateKeyPairHandler : IRequestHandler<CreatePublicPrivateKeyPairRequest, CreatePublicPrivateKeyPairResponse>
{
    private readonly ILogger<CreatePublicPrivateKeyPairHandler> _logger;

    public CreatePublicPrivateKeyPairHandler(ILogger<CreatePublicPrivateKeyPairHandler> logger)
    {
        _logger = logger;
    }

    public Task<CreatePublicPrivateKeyPairResponse> Handle(CreatePublicPrivateKeyPairRequest request, CancellationToken cancellationToken)
    {
        var keyPair = PublicKeyPairGenerator.Generate();
        _logger.LogInformation(
            "Generated key with Public Key={PublicKey} and Private Key={PrivateKey}",
            Convert.ToBase64String(keyPair.publicKey),
            Convert.ToBase64String(keyPair.privateKey)
        );

        var response = new CreatePublicPrivateKeyPairResponse(keyPair.publicKey, keyPair.privateKey);

        return Task.FromResult(response);
    }
}