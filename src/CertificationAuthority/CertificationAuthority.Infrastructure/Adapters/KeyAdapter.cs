using CertificationAuthority.Domain.ValueObjects;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace CertificationAuthority.Infrastructure.Adapters;

public static class KeyAdapter
{
    public static (PublicKey, PrivateKey) GenerateRsaKeyPair(int keySize = 2048)
    {
        var keyGen = new RsaKeyPairGenerator();
        keyGen.Init(new KeyGenerationParameters(new SecureRandom(), keySize));
        var keyPair = keyGen.GenerateKeyPair();

        return (FromBouncyCastlePublicKey(keyPair.Public), FromBouncyCastlePrivateKey(keyPair.Private));
    }

    public static PublicKey FromBouncyCastlePublicKey(AsymmetricKeyParameter publicKey)
    {
        byte[] keyBytes = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(publicKey).GetDerEncoded();
        return new PublicKey(keyBytes);
    }

    public static PrivateKey FromBouncyCastlePrivateKey(AsymmetricKeyParameter privateKey)
    {
        byte[] keyBytes = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey).GetDerEncoded();
        return new PrivateKey(keyBytes);
    }

    public static AsymmetricKeyParameter ToBouncyCastlePublicKey(PublicKey publicKey)
    {
        return PublicKeyFactory.CreateKey(publicKey.ToDer());
    }

    public static AsymmetricKeyParameter ToBouncyCastlePrivateKey(PrivateKey privateKey)
    {
        return PrivateKeyFactory.CreateKey(privateKey.ToDer());
    }
}