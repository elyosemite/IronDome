using CertificationAuthority.Domain.Enumerations;
using CertificationAuthority.Domain.ValueObjects;

namespace CertificationAuthority.Domain.Certificate;

public class Csr
{
    public SubjectDN SubjectDN { get; }
    public PublicKey PublicKey { get; }
    public PrivateKey PrivateKey { get; }
    public SignatureAlgorithm SignatureAlgorithm { get; }
    public FilePath PublicKeyFilePath { get; }
    public FilePath PrivateKeyFilePath { get; }

    public Csr(string subjectDN, string publicKey, string privateKey, SignatureAlgorithmEnum signatureAlgorithm, string publicKeyFilePath, string privateKeyFilePath)
    {
        SubjectDN = new SubjectDN(subjectDN);
        PublicKey = new PublicKey(publicKey);
        PrivateKey = new PrivateKey(privateKey);
        SignatureAlgorithm = new SignatureAlgorithm(signatureAlgorithm);
        PublicKeyFilePath = new FilePath(publicKeyFilePath);
        PrivateKeyFilePath = new FilePath(privateKeyFilePath);
    }
}
