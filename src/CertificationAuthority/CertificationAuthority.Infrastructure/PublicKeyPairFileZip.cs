using System.IO.Compression;

namespace CertificationAuthority.Infrastructure;

public class PublicKeyPairFileZip : IFileZip<Foo>
{
    public async Task<byte[]> Zip(Foo entity)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                var publicKeyEntry = zipArchive.CreateEntry("public_key.pem");
                using (var publicKeyStream = publicKeyEntry.Open())
                {
                    await publicKeyStream.WriteAsync(entity.PublicKey, 0, entity.PublicKey.Length);
                }

                var privateKeyEntry = zipArchive.CreateEntry("private_key.der");
                using (var privateKeyStream = privateKeyEntry.Open())
                {
                    await privateKeyStream.WriteAsync(entity.PrivateKey, 0, entity.PrivateKey.Length);
                }
            }

            memoryStream.Seek(0, SeekOrigin.Begin);

            return memoryStream.ToArray();
        }
    }
}
