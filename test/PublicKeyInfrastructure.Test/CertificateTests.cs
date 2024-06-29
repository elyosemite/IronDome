namespace PublicKeyInfrastructure.Test;

using NUnit.Framework;
using Org.BouncyCastle.X509;
using System.IO;
using Org.BouncyCastle.Crypto;

[TestFixture]
public class CertificateTests
{
    private AsymmetricCipherKeyPair keyPair;
    private X509Certificate certificate;
    private const string pemPath = "test_certificate.pem";
    private const string derPath = "test_certificate.der";

    [SetUp]
    public void Setup()
    {
        keyPair = new GeneratePairKey().Generate();
        certificate = new CertificateGenerator(keyPair).GenerateSelfSignedCertificate("CN=AWS", "CN=AWS");
    }

    [Test]
    public void GenerateKeyPair_ShouldReturnValidKeyPair()
    {
        Assert.IsNotNull(keyPair);
        Assert.IsNotNull(keyPair.Private);
        Assert.IsNotNull(keyPair.Public);
    }

    [Test]
    public void GenerateCertificate_ShouldReturnValidCertificate()
    {
        Assert.IsNotNull(certificate);
        Assert.That(certificate.SubjectDN.ToString(), Is.EqualTo("CN=AWS"));
        Assert.That(certificate.SubjectDN, Is.EqualTo(certificate.IssuerDN)); // As it's self-signed
    }

    [Test]
    public void SaveCertificateAsPem_ShouldCreateFile()
    {
        certificate.SaveCertificateAsPem(keyPair.Private, pemPath);
        Assert.IsTrue(File.Exists(pemPath));
    }

    [Test]
    public void SaveCertificateAsDer_ShouldCreateFile()
    {
        certificate.SaveCertificateAsDer(derPath);
        Assert.IsTrue(File.Exists(derPath));
    }

    [Test]
    public void SavedPemFile_ShouldContainCertificateAndPrivateKey()
    {
        certificate.SaveCertificateAsPem(keyPair.Private, pemPath);
        string fileContent = File.ReadAllText(pemPath);
        Assert.IsTrue(fileContent.Contains("BEGIN CERTIFICATE"));
        Assert.IsTrue(fileContent.Contains("BEGIN RSA PRIVATE KEY"));
    }

    [TearDown]
    public void Cleanup()
    {
        if (File.Exists(pemPath)) File.Delete(pemPath);
        if (File.Exists(derPath)) File.Delete(derPath);
    }
}
