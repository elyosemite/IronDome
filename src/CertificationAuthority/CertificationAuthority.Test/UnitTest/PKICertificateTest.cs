using CertificationAuthority.Domain.Builders;
using CertificationAuthority.Domain.Factories;

namespace CertificationAuthority.Test.UnitTest;

public class Tests
{
    [Test]
    public void CreateCertificate_UsingFactory_ReturnsValidCertificate()
    {
        // Arrange
        var factory = new PKICertificateFactory();
        var issuerDN = "CN=Issuer";
        var serialNumber = "123456789";
        var notBefore = DateTime.UtcNow;
        var notAfter = DateTime.UtcNow.AddYears(1);
        var subjectDN = "CN=Subject";
        var publicKey = "PublicKey";
        var signatureAlgorithm = "SHA256WithRSA";

        // Act
        var certificate = factory.Factory(issuerDN, serialNumber, notBefore, notAfter, subjectDN, publicKey, signatureAlgorithm);

        // Assert
        Assert.IsNotNull(certificate);
        Assert.That(issuerDN, Is.EqualTo(certificate.IssuerDN));
        Assert.That(serialNumber, Is.EqualTo(certificate.SerialNumber));
        Assert.That(notBefore, Is.EqualTo(certificate.NotBefore));
        Assert.That(notAfter, Is.EqualTo(certificate.NotAfter));
        Assert.That(subjectDN, Is.EqualTo(certificate.SubjectDN));
        Assert.That(publicKey, Is.EqualTo(certificate.PublicKey));
        Assert.That(signatureAlgorithm, Is.EqualTo(certificate.SignatureAlgorithm));
    }

    [Test]
    public void CreateCertificate_WithIdentifier_UsingFactory_ReturnsValidCertificate()
    {
        // Arrange
        var factory = new PKICertificateFactory();
        var id = Guid.NewGuid();
        var issuerDN = "CN=Issuer";
        var serialNumber = "123456789";
        var notBefore = DateTime.UtcNow;
        var notAfter = DateTime.UtcNow.AddYears(1);
        var subjectDN = "CN=Subject";
        var publicKey = "PublicKey";
        var signatureAlgorithm = "SHA256WithRSA";

        // Act
        var certificate = factory.Factory(id, issuerDN, serialNumber, notBefore, notAfter, subjectDN, publicKey, signatureAlgorithm);

        // Assert
        Assert.IsNotNull(certificate);
        Assert.That(id, Is.EqualTo(certificate.Id));
        Assert.That(issuerDN, Is.EqualTo(certificate.IssuerDN));
        Assert.That(serialNumber, Is.EqualTo(certificate.SerialNumber));
        Assert.That(notBefore, Is.EqualTo(certificate.NotBefore));
        Assert.That(notAfter, Is.EqualTo(certificate.NotAfter));
        Assert.That(subjectDN, Is.EqualTo(certificate.SubjectDN));
        Assert.That(publicKey, Is.EqualTo(certificate.PublicKey));
        Assert.That(signatureAlgorithm, Is.EqualTo(certificate.SignatureAlgorithm));
    }

    [Test]
    public void CreateCertificate_UsingBuilder_ReturnsValidCertificate()
    {
        // Arrange
        var builder = new CertificateBuilder();
        var issuerDN = "CN=Issuer";
        var serialNumber = "123456789";
        var notBefore = DateTime.UtcNow;
        var notAfter = DateTime.UtcNow.AddYears(1);
        var subjectDN = "CN=Subject";
        var publicKey = "PublicKey";
        var signatureAlgorithm = "SHA256WithRSA";

        // Act
        var certificate = builder.WithIssuerDN(issuerDN)
                                 .WithSerialNumber(serialNumber)
                                 .WithNotBefore(notBefore)
                                 .WithNotAfter(notAfter)
                                 .WithSubjectDN(subjectDN)
                                 .WithPublicKey(publicKey)
                                 .WithSignatureAlgorithm(signatureAlgorithm)
                                 .Build();

        // Assert
        Assert.IsNotNull(certificate);
        Assert.That(issuerDN, Is.EqualTo(certificate.IssuerDN));
        Assert.That(serialNumber, Is.EqualTo(certificate.SerialNumber));
        Assert.That(notBefore, Is.EqualTo(certificate.NotBefore));
        Assert.That(notAfter, Is.EqualTo(certificate.NotAfter));
        Assert.That(subjectDN, Is.EqualTo(certificate.SubjectDN));
        Assert.That(publicKey, Is.EqualTo(certificate.PublicKey));
        Assert.That(signatureAlgorithm, Is.EqualTo(certificate.SignatureAlgorithm));
    }

    [Test]
    public void CreateCertificate_WithIdentifier_UsingBuilder_ReturnsValidCertificate()
    {
        // Arrange
        var builder = new CertificateBuilder();
        var id = Guid.NewGuid();
        var issuerDN = "CN=Issuer";
        var serialNumber = "123456789";
        var notBefore = DateTime.UtcNow;
        var notAfter = DateTime.UtcNow.AddYears(1);
        var subjectDN = "CN=Subject";
        var publicKey = "PublicKey";
        var signatureAlgorithm = "SHA256WithRSA";

        // Act
        var certificate = builder.WithIdentifier(id)
                                 .WithIssuerDN(issuerDN)
                                 .WithSerialNumber(serialNumber)
                                 .WithNotBefore(notBefore)
                                 .WithNotAfter(notAfter)
                                 .WithSubjectDN(subjectDN)
                                 .WithPublicKey(publicKey)
                                 .WithSignatureAlgorithm(signatureAlgorithm)
                                 .Build();

        // Assert
        Assert.IsNotNull(certificate);
        Assert.That(id, Is.EqualTo(certificate.Id));
        Assert.That(issuerDN, Is.EqualTo(certificate.IssuerDN));
        Assert.That(serialNumber, Is.EqualTo(certificate.SerialNumber));
        Assert.That(notBefore, Is.EqualTo(certificate.NotBefore));
        Assert.That(notAfter, Is.EqualTo(certificate.NotAfter));
        Assert.That(subjectDN, Is.EqualTo(certificate.SubjectDN));
        Assert.That(publicKey, Is.EqualTo(certificate.PublicKey));
        Assert.That(signatureAlgorithm, Is.EqualTo(certificate.SignatureAlgorithm));
    }

    [Test]
    public void CreateCertificate_WithInvalidDates_ThrowsException()
    {
        // Arrange
        var factory = new PKICertificateFactory();
        var issuerDN = "CN=Issuer";
        var serialNumber = "123456789";
        var notBefore = DateTime.UtcNow;
        var notAfter = DateTime.UtcNow.AddYears(-1); // Invalid: notAfter is before notBefore
        var subjectDN = "CN=Subject";
        var publicKey = "PublicKey";
        var signatureAlgorithm = "SHA256WithRSA";

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            factory.Factory(issuerDN, serialNumber, notBefore, notAfter, subjectDN, publicKey, signatureAlgorithm));
    }
}