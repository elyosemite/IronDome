using CertificationAuthority.Domain.Certificate;
using CertificationAuthority.Domain.Enumerations;

namespace CertificationAuthority.Domain.Factories;

public class PKICertificateFactory : ICertificateFactory
{
    public PkiCertificate Factory(string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, string publicKey, SignatureAlgorithmEnum signatureAlgorithm) => 
        new PkiCertificate(issuerDN, serialNumber, notBefore, notAfter, subjectDN, publicKey, signatureAlgorithm);

    public PkiCertificate Factory(Guid identifier, string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, string publicKey, SignatureAlgorithmEnum signatureAlgorithm) =>
        new PkiCertificate(identifier, issuerDN, serialNumber, notBefore, notAfter, subjectDN, publicKey, signatureAlgorithm);
}
