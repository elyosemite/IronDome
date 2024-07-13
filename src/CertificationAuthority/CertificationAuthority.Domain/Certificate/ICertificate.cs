using CertificationAuthority.Domain.ValueObjects;

namespace CertificationAuthority.Domain.Certificate;

public interface ICertificate
{
    IssuerDN IssuerDN { get; }
    SerialNumber SerialNumber { get; }
    DateTime NotBefore { get; }
    DateTime NotAfter { get; }
    SubjectDN SubjectDN { get; }
    PublicKey PublicKey { get; }
    SignatureAlgorithm SignatureAlgorithm { get; }
}
