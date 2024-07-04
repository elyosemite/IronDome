using Org.BouncyCastle.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;

public static class CertificateX509Extension
{
    public static void SaveCertificateAsPem(this X509Certificate certificate, AsymmetricKeyParameter privateKey, string path)
    {
        using (var writer = new StreamWriter(path))
        {
            var pemWriter = new PemWriter(writer);
            pemWriter.WriteObject(certificate);
            pemWriter.WriteObject(privateKey);
        }
    }

    public static void SaveCertificateAsDer(this X509Certificate certificate, string path)
    {
        var derEncoded = certificate.GetEncoded();
        File.WriteAllBytes(path, derEncoded);
    }
}