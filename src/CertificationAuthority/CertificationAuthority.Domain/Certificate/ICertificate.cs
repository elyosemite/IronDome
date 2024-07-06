namespace CertificationAuthority.Domain.Certificate;

public interface ICertificate
{
    string IssuerDN { get; }
    string SerialNumber { get; }
    DateTime NotBefore { get; }
    DateTime NotAfter { get; }
    string SubjectDN { get; }
    string PublicKey { get; }
    string SignatureAlgorithm { get; }
}
