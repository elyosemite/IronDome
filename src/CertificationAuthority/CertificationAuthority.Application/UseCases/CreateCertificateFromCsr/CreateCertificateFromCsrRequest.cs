using CertificationAuthority.Domain.Enumerations;
using MediatR;

namespace CertificationAuthority.Application.UseCases.CreateCertificateFromCsr;

public record CreateCertificateFromCsrRequest(
    string IssuerDN,
    string SerialNumber,
    DateTime NotBefore,
    DateTime NotAfter,
    string SubjectDN,
    string PublicKey,
    string SenderPrivateKey,
    SignatureAlgorithmEnum SignatureAlgorithm) : IRequest<CreateCertificateFromCsrResponse>;
