using System.Text;
using CertificationAuthority.Domain.ValueObjects;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;

namespace CertificationAuthority.Infrastructure.Extensions;

public static class KeyExtension
{
    public static PublicKey ToPublicKey(this byte[] der) => new PublicKey(der);
    public static PublicKey ToPublicKeyFromBase64(this string base64) => new PublicKey(Convert.FromBase64String(base64));
    //public static PrivateKey ToPrivateKey(this byte[] der) => new PrivateKey(der);
    //public static PrivateKey ToPrivateKeyFromBase64(this string base64) => new PrivateKey(Convert.FromBase64String(base64));
    //public static SymmetricKey ToSymmetricKey(this byte[] der) => new SymmetricKey(der);
    //public static SymmetricKey ToSymmetricKeyFromBase64(this string base64) => new SymmetricKey(Convert.FromBase64String(base64));



    public static AsymmetricCipherKeyPair Generate()
    {
        var keyGenerationParameters = new KeyGenerationParameters(new SecureRandom(), 2048);
        var keyPairGenerator = new RsaKeyPairGenerator();
        keyPairGenerator.Init(keyGenerationParameters);
        return keyPairGenerator.GenerateKeyPair();
    }

    public static string ConvertToBase64(this AsymmetricKeyParameter key)
    {
        using (var sw = new StringWriter())
        using (var pemWriter = new PemWriter(sw))
        {
            pemWriter.WriteObject(key);
            string pemString = sw.ToString();

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(pemString));
        }
    }

    public static byte[] ConvertToPem(AsymmetricKeyParameter key)
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

    public static AsymmetricKeyParameter ParseFromPEMPublicKey(string publicKey)
    {
        using (var reader = new StringReader(publicKey))
        {
            var pemReader = new PemReader(reader);
            object keyObject = pemReader.ReadObject();

            if (keyObject is AsymmetricKeyParameter keyParameter)
                return keyParameter;

            throw new InvalidOperationException("Invalid formato para Public Key - PEM.");
        }
    }

    public static AsymmetricKeyParameter ParseFromPEMPrivateKey(string privateKey)
    {
        using (var reader = new StringReader(privateKey))
        {
            var pemReader = new PemReader(reader);
            object keyObject = pemReader.ReadObject();

            if (keyObject is AsymmetricCipherKeyPair keyPair)
                return keyPair.Private;
            else if (keyObject is AsymmetricKeyParameter keyParameter)
                return keyParameter;

            throw new InvalidOperationException("Invalid formato para Private Key - PEM.");
        }
    }

    // Novos métodos
    public static byte[] ToDer(this AsymmetricKeyParameter key)
    {
        return PrivateKeyInfoFactory.CreatePrivateKeyInfo(key).ToAsn1Object().GetDerEncoded();
    }

    public static string ToPem(this AsymmetricKeyParameter key, string header)
    {
        using (var sw = new StringWriter())
        using (var pemWriter = new PemWriter(sw))
        {
            pemWriter.WriteObject(key);
            return sw.ToString();
        }
    }

    public static byte[] ToDer(this string pemString)
    {
        using (var reader = new StringReader(pemString))
        {
            var pemReader = new PemReader(reader);
            var keyObject = pemReader.ReadObject();

            if (keyObject is AsymmetricKeyParameter keyParameter)
                return keyParameter.ToDer();
            else if (keyObject is AsymmetricCipherKeyPair keyPair)
                return keyPair.Private.ToDer();

            throw new InvalidOperationException("Invalid key format.");
        }
    }

    public static AsymmetricKeyParameter ToAsymmetricKeyParameter(this byte[] derData)
    {
        using (var reader = new StringReader(Convert.ToBase64String(derData)))
        {
            var pemReader = new PemReader(reader);
            var keyObject = pemReader.ReadObject();

            if (keyObject is AsymmetricKeyParameter keyParameter)
                return keyParameter;
            else if (keyObject is AsymmetricCipherKeyPair keyPair)
                return keyPair.Private;

            throw new InvalidOperationException("Invalid key format.");
        }
    }
}