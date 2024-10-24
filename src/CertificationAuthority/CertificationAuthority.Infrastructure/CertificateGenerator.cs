
using CertificationAuthority.Domain.Certificate;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

using Org.BouncyCastle.X509;

namespace CertificationAuthority.Infrastructure;

public class CertificateGenerator : ICertificateGenerator
{
    public byte[] X509CreateCertificate(PKICertificate pkiCertificate, byte[] senderPrivateKey)
    {
        var certificate = new X509V3CertificateGenerator();
        certificate.SetSerialNumber(BigInteger.ProbablePrime(120, new Random()));
        certificate.SetSubjectDN(new X509Name(pkiCertificate.SubjectDN.Value));
        certificate.SetIssuerDN(new X509Name(pkiCertificate.IssuerDN.Value));
        certificate.SetNotBefore(pkiCertificate.NotBefore);
        certificate.SetNotAfter(pkiCertificate.NotAfter);

        using (StringReader stringReader = new StringReader(pkiCertificate.PublicKey.Value))
        {
            PemReader pemReader = new PemReader(stringReader);

            var publicKeyObject = pemReader.ReadObject();
            if (publicKeyObject is AsymmetricKeyParameter publicKey)
            {
                certificate.SetPublicKey(publicKey);
            }
        }

        PrivateKeyInfo privateKeyInfo = PrivateKeyInfo.GetInstance(senderPrivateKey);

        AsymmetricKeyParameter privateKey = PrivateKeyFactory.CreateKey(privateKeyInfo);

        var signatureFactory = new Asn1SignatureFactory("SHA256WithRSA", privateKey);

        return certificate.Generate(signatureFactory).GetEncoded();
    }
}
