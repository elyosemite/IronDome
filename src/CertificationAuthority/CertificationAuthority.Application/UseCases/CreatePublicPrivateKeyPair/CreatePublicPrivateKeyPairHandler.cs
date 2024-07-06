using CertificationAuthority.Infrastructure;
using MediatR;

namespace CertificationAuthority.Application.UseCases.CreatePublicPrivateKeyPair;

public class CreatePublicPrivateKeyPairHandler : IRequestHandler<CreatePublicPrivateKeyPairRequest, CreatePublicPrivateKeyPairResponse>
{
    private readonly IPublicKeyPairGenerator _publicKeyPairGenerator;

    public CreatePublicPrivateKeyPairHandler(IPublicKeyPairGenerator publicKeyPairGenerator)
    {
        _publicKeyPairGenerator = publicKeyPairGenerator;
    }

    public Task<CreatePublicPrivateKeyPairResponse> Handle(CreatePublicPrivateKeyPairRequest request, CancellationToken cancellationToken)
    {
        var keyPair = _publicKeyPairGenerator.Generate();
        return Task.FromResult(new CreatePublicPrivateKeyPairResponse(keyPair.publicKey, keyPair.privateKey));
    }
}