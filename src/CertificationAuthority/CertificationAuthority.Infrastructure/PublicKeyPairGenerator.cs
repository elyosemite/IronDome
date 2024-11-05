using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;

namespace CertificationAuthority.Infrastructure;

public static class PublicKeyPairGenerator
{
    public static PublicKeyPair Generate()
    {
        var keyGenerationParameters = new KeyGenerationParameters(new SecureRandom(), 2048);
        var keyPairGenerator = new RsaKeyPairGenerator();
        keyPairGenerator.Init(keyGenerationParameters);
        var keyPair = keyPairGenerator.GenerateKeyPair();

        byte[] publicKey = ConvertToPem(keyPair.Public);

        var privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keyPair.Private);
        byte[] privateKey = privateKeyInfo.ToAsn1Object().GetDerEncoded();

        return new PublicKeyPair(publicKey, privateKey);
    }

    private static byte[] ConvertToPem(AsymmetricKeyParameter key)
    {
        using (var stringWriter = new StringWriter())
        {
            using (var pemWriter = new PemWriter(stringWriter))
            {
                pemWriter.WriteObject(key);
            }
            return Encoding.UTF8.GetBytes(stringWriter.ToString());
        }
    }
}