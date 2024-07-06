using Ardalis.SharedKernel;

namespace CertificationAuthority.Domain.Certificate;

//TODO - Cuidado com a obsessão por tipos primitivos
public class PKICertificate : EntityBase<Guid>, ICertificate
{
    public string IssuerDN { get; private set; }
    public string SerialNumber { get; private set; }
    public DateTime NotBefore { get; private set; }
    public DateTime NotAfter { get; private set;  }
    public string SubjectDN { get; private set; }
    public string PublicKey { get; private set; }
    public string SignatureAlgorithm { get; private set; } // "SHA256WithRSA";

    private PKICertificate()
    {
        Id = Guid.NewGuid();
    }

    public PKICertificate(string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, string publicKey, string signatureAlgorithm) : this()
    {
        NotBefore = notBefore;
        NotAfter = notAfter;

        var inconsistentDatesSpec = new InconsistentSpec();
        if (!inconsistentDatesSpec.IsSatisfiedBy(this)) throw new InvalidOperationException("Inconsistent Dates");

        SerialNumber = serialNumber;
        IssuerDN = issuerDN;
        SubjectDN = subjectDN;
        PublicKey = publicKey;
        SignatureAlgorithm = signatureAlgorithm;
    }

    public PKICertificate(Guid identifier, string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, string publicKey, string signatureAlgorithm)
        : this(issuerDN, serialNumber, notBefore, notAfter, subjectDN, publicKey, signatureAlgorithm)
    {
        Id = identifier;
    }
}
