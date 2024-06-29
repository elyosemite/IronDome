using Org.BouncyCastle.X509;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto;

public class CertificateGenerator
{
    private readonly AsymmetricCipherKeyPair _keyPair;

    public CertificateGenerator(AsymmetricCipherKeyPair keyPair)
    {
        _keyPair = keyPair;
    }

    public X509Certificate GenerateSelfSignedCertificate(string subjectName, string issuerName)
    {
        var certificate = new CertificateX509V3(_keyPair).CreateCertificateX509(subjectName, issuerName);

        var signatureFactory = new Asn1SignatureFactory("SHA256WithRSA", _keyPair.Private);
        return certificate.Generate(signatureFactory);
    }
}
