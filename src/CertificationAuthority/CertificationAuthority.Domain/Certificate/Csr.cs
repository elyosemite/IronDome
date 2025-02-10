using CertificationAuthority.Domain.ValueObjects;

namespace CertificationAuthority.Domain.Certificate;

public class Csr
{
    SubjectDN SubjectDN { get; }
    PublicKey PublicKey { get; }
    PrivateKey PrivateKey { get; }
    SignatureAlgorithm SignatureAlgorithm { get; }
    FilePath PublicKeyFilePath { get; }
    FilePath PrivateKeyFilePath { get; }

    public Csr(SubjectDN subjectDN, PublicKey publicKey, PrivateKey privateKey, SignatureAlgorithm signatureAlgorithm, FilePath publicKeyFilePath, FilePath privateKeyFilePath)
    {
        SubjectDN = subjectDN;
        PublicKey = publicKey;
        PrivateKey = privateKey;
        SignatureAlgorithm = signatureAlgorithm;
        PublicKeyFilePath = publicKeyFilePath;
        PrivateKeyFilePath = privateKeyFilePath;
    }
}
