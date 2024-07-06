using CertificationAuthority.Domain.Certificate;

namespace CertificationAuthority.Infrastructure;

public interface ICertificateGenerator
{
    byte[] X509CreateCertificate(PKICertificate pkiCertificate, byte[] senderPrivateKey);
}
