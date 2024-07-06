namespace CertificationAuthority.Domain.Certificate;

public interface ICertificate
{
    public string IssuerDN { get; }
    public string SerialNumber { get; }
    public DateTime NotBefore { get; }
    public DateTime NotAfter { get; }
    public string SubjectDN { get; }
    public string PublicKey { get; }
    public string SignatureAlgorithm { get; }
}
