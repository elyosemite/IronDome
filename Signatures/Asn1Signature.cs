using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.X509;

public class Asn1Signature
{
    private readonly AsymmetricCipherKeyPair _keyPair;
    private readonly CertificateX509V3 _certificateGenerator;

    public Asn1Signature(GeneratePairKey generator, CertificateX509V3 certificateGenerator)
    {
        _keyPair = generator.Generate();
        _certificateGenerator = certificateGenerator;
    }

    public X509Certificate GenerateSelfSignedCertificate(string subjectName, string issuerName)
    {
        var signatureFactory = new Asn1SignatureFactory("SHA256WithRSA", _keyPair.Private);

        return _certificateGenerator.CreateCertificateX509(subjectName, issuerName).Generate(signatureFactory);
    }
}
