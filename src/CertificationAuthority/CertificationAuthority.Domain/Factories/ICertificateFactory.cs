using CertificationAuthority.Domain.Certificate;

namespace CertificationAuthority.Domain.Factories;

public interface ICertificateFactory
{
    PKICertificate Factory(string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, string publicKey, string signatureAlgorithm);
    PKICertificate Factory(Guid identifier, string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, string publicKey, string signatureAlgorithm);
}
