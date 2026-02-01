using CertificationAuthority.Domain.Certificate;
using CertificationAuthority.Domain.Enumerations;

namespace CertificationAuthority.Domain.Factories;

public interface ICertificateFactory
{
    PkiCertificate Factory(string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, string publicKey, SignatureAlgorithmEnum signatureAlgorithm);
    PkiCertificate Factory(Guid identifier, string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, string publicKey, SignatureAlgorithmEnum signatureAlgorithm);
}
