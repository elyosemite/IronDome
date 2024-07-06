using CertificationAuthority.Domain.Certificate;

namespace CertificationAuthority.Domain.Factories;

public interface ICertificateFactory
{
    PKICertificate Factory(string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, string publicKey, string signatureAlgorithm);
    PKICertificate Factory(Guid identifier, string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, string publicKey, string signatureAlgorithm);
}

public class PKICertificateFactory : ICertificateFactory
{
    public PKICertificate Factory(string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, string publicKey, string signatureAlgorithm) => 
        new PKICertificate(issuerDN, serialNumber, notBefore, notAfter, subjectDN, publicKey, signatureAlgorithm);

    public PKICertificate Factory(Guid identifier, string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, string publicKey, string signatureAlgorithm) =>
        new PKICertificate(identifier, issuerDN, serialNumber, notBefore, notAfter, subjectDN, publicKey, signatureAlgorithm);
}
