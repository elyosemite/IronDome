using CertificationAuthority.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CertificationAuthority.Application.UseCases.CreatePublicPrivateKeyPair;

public class CreatePublicPrivateKeyPairHandler : IRequestHandler<CreatePublicPrivateKeyPairRequest, CreatePublicPrivateKeyPairResponse>
{
    private readonly IPublicKeyPairGenerator _publicKeyPairGenerator;
    private readonly ILogger<CreatePublicPrivateKeyPairHandler> _logger;

    public CreatePublicPrivateKeyPairHandler(IPublicKeyPairGenerator publicKeyPairGenerator, ILogger<CreatePublicPrivateKeyPairHandler> logger)
    {
        _publicKeyPairGenerator = publicKeyPairGenerator;
        _logger = logger;
    }

    public Task<CreatePublicPrivateKeyPairResponse> Handle(CreatePublicPrivateKeyPairRequest request, CancellationToken cancellationToken)
    {
        var keyPair = _publicKeyPairGenerator.Generate();
        _logger.LogInformation($"Generated key {keyPair}");

        var response = new CreatePublicPrivateKeyPairResponse(keyPair.publicKey, keyPair.privateKey);

        _logger.LogInformation($"Response {response}");
        return Task.FromResult(response);
    }
}