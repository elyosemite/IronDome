using Ardalis.SharedKernel;
using CertificationAuthority.Domain.Enumerations;
using CertificationAuthority.Domain.ValueObjects;

namespace CertificationAuthority.Domain.Certificate;

//TODO - Cuidado com a obsessão por tipos primitivos
// AggregateRoot com Repository do DDD (Eric Evans e Vaughn Vernon)
public class PKICertificate : EntityBase<Guid>, ICertificate
{
    public IssuerDN IssuerDN { get; }
    public SerialNumber SerialNumber { get; }
    public DateTime NotBefore { get; }
    public DateTime NotAfter { get; }
    public SubjectDN SubjectDN { get; }
    public PublicKey PublicKey { get; }
    public SignatureAlgorithm SignatureAlgorithm { get; } // TODO - "SHA256WithRSA"; Smart Enums do DDD

    public PKICertificate(string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, string publicKey, SignatureAlgorithmEnum signatureAlgorithm)
    {
        Id = Guid.NewGuid();
        NotBefore = notBefore;
        NotAfter = notAfter;

        var inconsistentDatesSpec = new InconsistentSpec();
        if (!inconsistentDatesSpec.IsSatisfiedBy(this)) throw new InvalidOperationException("Inconsistent Dates");

        SerialNumber = new SerialNumber(serialNumber);
        IssuerDN = new IssuerDN(issuerDN);
        SubjectDN = new SubjectDN(subjectDN);
        PublicKey = new PublicKey(publicKey);
        SignatureAlgorithm = new SignatureAlgorithm(signatureAlgorithm);
    }

    public PKICertificate(string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, byte[] publicKey, SignatureAlgorithmEnum signatureAlgorithm)
    {
        Id = Guid.NewGuid();
        NotBefore = notBefore;
        NotAfter = notAfter;

        var inconsistentDatesSpec = new InconsistentSpec();
        if (!inconsistentDatesSpec.IsSatisfiedBy(this)) throw new InvalidOperationException("Inconsistent Dates");

        SerialNumber = new SerialNumber(serialNumber);
        IssuerDN = new IssuerDN(issuerDN);
        SubjectDN = new SubjectDN(subjectDN);
        PublicKey = new PublicKey(publicKey);
        SignatureAlgorithm = new SignatureAlgorithm(signatureAlgorithm);
    }

    public PKICertificate(Guid identifier, string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, string publicKey, SignatureAlgorithmEnum signatureAlgorithm)
    {
        Id = identifier;
        NotBefore = notBefore;
        NotAfter = notAfter;

        var inconsistentDatesSpec = new InconsistentSpec();
        if (!inconsistentDatesSpec.IsSatisfiedBy(this)) throw new InvalidOperationException("Inconsistent Dates");

        SerialNumber = new SerialNumber(serialNumber);
        IssuerDN = new IssuerDN(issuerDN);
        SubjectDN = new SubjectDN(subjectDN);
        PublicKey = new PublicKey(publicKey);
        SignatureAlgorithm = new SignatureAlgorithm(signatureAlgorithm);
    }

    public PKICertificate(Guid identifier, string issuerDN, string serialNumber, DateTime notBefore, DateTime notAfter, string subjectDN, byte[] publicKey, SignatureAlgorithmEnum signatureAlgorithm)
    {
        Id = identifier;
        NotBefore = notBefore;
        NotAfter = notAfter;

        var inconsistentDatesSpec = new InconsistentSpec();
        if (!inconsistentDatesSpec.IsSatisfiedBy(this)) throw new InvalidOperationException("Inconsistent Dates");

        SerialNumber = new SerialNumber(serialNumber);
        IssuerDN = new IssuerDN(issuerDN);
        SubjectDN = new SubjectDN(subjectDN);
        PublicKey = new PublicKey(publicKey);
        SignatureAlgorithm = new SignatureAlgorithm(signatureAlgorithm);
    }
}
