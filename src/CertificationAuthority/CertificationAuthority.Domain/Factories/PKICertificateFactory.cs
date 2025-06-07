using CertificationAuthority.Domain.Certificate;
using CertificationAuthority.Domain.Enumerations;

namespace CertificationAuthority.Domain.Factories;

public class PKICertificateFactory : ICertificateFactory
{
    public PKICertificate Factory(string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, byte[] publicKey, SignatureAlgorithmEnum signatureAlgorithm) =>
        new PKICertificate(issuerDN, serialNumber, notBefore, notAfter, subjectDN, publicKey, signatureAlgorithm);

    public PKICertificate Factory(string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, string publicKey, SignatureAlgorithmEnum signatureAlgorithm) =>
        new PKICertificate(issuerDN, serialNumber, notBefore, notAfter, subjectDN, publicKey, signatureAlgorithm);

    public PKICertificate Factory(Guid identifier, string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, string publicKey, SignatureAlgorithmEnum signatureAlgorithm) =>
        new PKICertificate(identifier, issuerDN, serialNumber, notBefore, notAfter, subjectDN, publicKey, signatureAlgorithm);
}
