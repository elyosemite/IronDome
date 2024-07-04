using Org.BouncyCastle.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;

public static class KeyPairExtension
{
    public static void SaveCertificateAsPem(this GeneratePairKey generatePairKey, X509Certificate certificate, AsymmetricKeyParameter privateKey, string path)
    {
        using (var writer = new StreamWriter(path))
        {
            var pemWriter = new PemWriter(writer);
            pemWriter.WriteObject(certificate);
            pemWriter.WriteObject(privateKey);
        }
    }
}