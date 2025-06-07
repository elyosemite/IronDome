using CertificationAuthority.Domain.Certificate;
using CertificationAuthority.Domain.Enumerations;

namespace CertificationAuthority.Domain.Factories;

public interface ICertificateFactory
{
    PKICertificate Factory(string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, byte[] publicKey, SignatureAlgorithmEnum signatureAlgorithm);
    PKICertificate Factory(string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, string publicKey, SignatureAlgorithmEnum signatureAlgorithm);
    PKICertificate Factory(Guid identifier, string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, byte[] publicKey, SignatureAlgorithmEnum signatureAlgorithm);
    PKICertificate Factory(Guid identifier, string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, string publicKey, SignatureAlgorithmEnum signatureAlgorithm);
}
