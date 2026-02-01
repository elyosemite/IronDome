using CertificationAuthority.Domain.Enumerations;
using CertificationAuthority.Domain.ValueObjects;

namespace CertificationAuthority.Domain.Certificate;

public class Csr(
    string subjectDn,
    string publicKey,
    string privateKey,
    SignatureAlgorithmEnum signatureAlgorithm,
    string publicKeyFilePath,
    string privateKeyFilePath)
{
    public SubjectDN SubjectDn { get; } = new(subjectDn);
    public PublicKey PublicKey { get; } = new(publicKey);
    public PrivateKey PrivateKey { get; } = new(privateKey);
    public SignatureAlgorithm SignatureAlgorithm { get; } = new(signatureAlgorithm);
    public FilePath PublicKeyFilePath { get; } = new(publicKeyFilePath);
    public FilePath PrivateKeyFilePath { get; } = new(privateKeyFilePath);
}
