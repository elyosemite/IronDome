using Org.BouncyCastle.X509;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto;

public class CertificateX509V3
{
    public readonly AsymmetricCipherKeyPair KeyPair;

    public CertificateX509V3(AsymmetricCipherKeyPair keyPair)
    {
        KeyPair = keyPair;
    }

    public X509V3CertificateGenerator CreateCertificateX509(string subjectName, string issuerName)
    {
        var generator = new X509V3CertificateGenerator();
        generator.SetSerialNumber(BigInteger.ProbablePrime(120, new Random()));
        generator.SetSubjectDN(new X509Name(subjectName));
        generator.SetIssuerDN(new X509Name(issuerName));
        generator.SetNotBefore(DateTime.UtcNow.Date);
        generator.SetNotAfter(DateTime.UtcNow.Date.AddYears(1));
        generator.SetPublicKey(KeyPair.Public);
        return generator;
    }
}
