using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;

public class GeneratePairKey
{
    public AsymmetricCipherKeyPair Generate()
    {
        var keyGenerationParameters = new KeyGenerationParameters(new SecureRandom(), 2048);
        var keyPairGenerator = new RsaKeyPairGenerator();
        keyPairGenerator.Init(keyGenerationParameters);
        var keyPair = keyPairGenerator.GenerateKeyPair();
        return keyPair;
    }
}