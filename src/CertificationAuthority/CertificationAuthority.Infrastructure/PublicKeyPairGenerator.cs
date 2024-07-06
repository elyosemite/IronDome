using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace CertificationAuthority.Infrastructure;

public class PublicKeyPairGenerator : IPublicKeyPairGenerator
{
    public PublicKeyPair Generate()
    {
        var keyGenerationParameters = new KeyGenerationParameters(new SecureRandom(), 2048);
        var keyPairGenerator = new RsaKeyPairGenerator();
        keyPairGenerator.Init(keyGenerationParameters);
        var keyPair = keyPairGenerator.GenerateKeyPair();

        var publicKeyInfo = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keyPair.Public);
        byte[] publicKey = publicKeyInfo.ToAsn1Object().GetDerEncoded();

        var privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keyPair.Private);
        byte[] privateKey = privateKeyInfo.ToAsn1Object().GetDerEncoded();


        return new PublicKeyPair(publicKey, privateKey);
    }
}