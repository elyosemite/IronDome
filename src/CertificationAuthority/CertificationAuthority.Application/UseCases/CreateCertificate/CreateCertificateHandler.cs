using System.Text;
using CertificationAuthority.Domain.Builders;
using CertificationAuthority.Infrastructure;
using MediatR;

namespace CertificationAuthority.Application.UseCases.CreateCertificate;

public class CreateCertificateHandler : IRequestHandler<CreateCertificateRequest, CreateCertificateResponse>
{
    private readonly ICertificateGenerator _certificateGenerator;
    private readonly ICertificateBuilder _certificateBuilder;

    public CreateCertificateHandler(ICertificateGenerator certificateGenerator, ICertificateBuilder certificateBuilder)
    {
        _certificateGenerator = certificateGenerator;
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
        var senderPrivateKey = Encoding.UTF8.GetBytes(request.SenderPrivateKey);
        var certificate = _certificateGenerator.X509CreateCertificate(domainCertificate, senderPrivateKey);

        return Task.FromResult(new CreateCertificateResponse(certificate));
    }
}
