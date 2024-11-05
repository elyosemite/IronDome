using CertificationAuthority.Domain.Builders;
using CertificationAuthority.Infrastructure;
using MediatR;

namespace CertificationAuthority.Application.UseCases.CreateCertificate;

public sealed class CreateCertificateHandler : IRequestHandler<CreateCertificateRequest, CreateCertificateResponse>
{
    private readonly ICertificateBuilder _certificateBuilder;

    public CreateCertificateHandler(ICertificateBuilder certificateBuilder)
    {
        _certificateBuilder = certificateBuilder;
    }

    public Task<CreateCertificateResponse> Handle(CreateCertificateRequest request, CancellationToken cancellationToken)
    {
        var domainCertificate = _certificateBuilder
            .WithIssuerDN(request.IssuerDN)
            .WithSerialNumber(request.SerialNumber)
            .WithSubjectDN(request.SubjectDN)
            .WithNotBefore(request.NotBefore)
            .WithNotAfter(request.NotAfter)
            .WithPublicKey(request.PublicKey)
            .WithSignatureAlgorithm(request.SignatureAlgorithm)
            .Build();

        var senderPrivateKey = Convert.FromBase64String(request.SenderPrivateKey);
        var certificate = CertificateGenerator.X509CreateCertificate(domainCertificate, senderPrivateKey);

        return Task.FromResult(new CreateCertificateResponse(certificate));
    }
}
