using CertificationAuthority.Domain.Builders;
using CertificationAuthority.Domain.Enumerations;
using CertificationAuthority.Domain.Factories;
using CertificationAuthority.Domain.ValueObjects;
using CertificationAuthority.Infrastructure.Adapters;

namespace CertificationAuthority.Test.UnitTest;

public class Tests
{
    private readonly PKICertificateFactory _certificateFactory = new();
    private readonly (PublicKey PublicKey, PrivateKey PrivateKey) _keyPair = KeyAdapter.GenerateRsaKeyPair();

    [Test]
    public void CreateCertificate_UsingFactory_ReturnsValidCertificate()
    {
        // Arrange
        var issuerDN = "Issuer";
        var serialNumber = "123456789";
        var notBefore = DateTime.UtcNow;
        var notAfter = DateTime.UtcNow.AddYears(1);
        var subjectDN = "Subject";
        var signatureAlgorithm = SignatureAlgorithmEnum.Sha256WithRsa;

        // Act
        var certificate = _certificateFactory.Factory(issuerDN, serialNumber, notBefore, notAfter, subjectDN, _keyPair.PublicKey.Value, signatureAlgorithm);

        // Assert
        Assert.IsNotNull(certificate);
        Assert.IsNotNull(certificate.IssuerDN);
        Assert.IsNotNull(certificate.SerialNumber);
        Assert.IsNotNull(certificate.SubjectDN);
        Assert.IsNotNull(certificate.PublicKey);
        Assert.IsNotNull(certificate.SignatureAlgorithm);
        Assert.That(KeyFormatValidator.IsDer(certificate.PublicKey.ToDer()), Is.EqualTo(true));
        Assert.That(KeyFormatValidator.IsPem(certificate.PublicKey.ToPem()), Is.EqualTo(true));
        Assert.That(KeyFormatValidator.IsBase64(certificate.PublicKey.ToBase64()), Is.EqualTo(true));
        Assert.That(KeyFormatValidator.IsBase64(certificate.PublicKey.Value), Is.EqualTo(true));
    }

    [Test]
    public void CreateCertificate_WithIdentifier_UsingFactory_ReturnsValidCertificate()
    {
        // Arrange
        var factory = new PKICertificateFactory();
        var id = Guid.NewGuid();
        var issuerDN = "Issuer";
        var serialNumber = "123456789";
        var notBefore = DateTime.UtcNow;
        var notAfter = DateTime.UtcNow.AddYears(1);
        var subjectDN = "Subject";
        var signatureAlgorithm = SignatureAlgorithmEnum.Sha256WithRsa;

        // Act
        var certificate = _certificateFactory.Factory(id, issuerDN, serialNumber, notBefore, notAfter, subjectDN, _keyPair.PublicKey.Value, signatureAlgorithm);

        // Assert
        Assert.IsNotNull(certificate);
        Assert.That(id, Is.EqualTo(certificate.Id));
        Assert.That($"CN={issuerDN}", Is.EqualTo(certificate.IssuerDN.Value));
        Assert.That(serialNumber, Is.EqualTo(certificate.SerialNumber.Value));
        Assert.That(notBefore, Is.EqualTo(certificate.NotBefore));
        Assert.That(notAfter, Is.EqualTo(certificate.NotAfter));
        Assert.That($"CN={subjectDN}", Is.EqualTo(certificate.SubjectDN.Value));
        Assert.That(signatureAlgorithm, Is.EqualTo(certificate.SignatureAlgorithm.Value));
    }

    [Test]
    public void CreateCertificate_UsingBuilder_ReturnsValidCertificate()
    {
        // Arrange
        var builder = new CertificateBuilder(_certificateFactory);
        var issuerDN = "Issuer";
        var serialNumber = "123456789";
        var notBefore = DateTime.UtcNow;
        var notAfter = DateTime.UtcNow.AddYears(1);
        var subjectDN = "Subject";
        var signatureAlgorithm = SignatureAlgorithmEnum.Sha256WithRsa;

        // Act
        var certificate = builder.WithIssuerDN(issuerDN)
                                 .WithSerialNumber(serialNumber)
                                 .WithNotBefore(notBefore)
                                 .WithNotAfter(notAfter)
                                 .WithSubjectDN(subjectDN)
                                 .WithPublicKey(_keyPair.PublicKey.Value)
                                 .WithSignatureAlgorithm(signatureAlgorithm)
                                 .Build();

        // Assert
        Assert.IsNotNull(certificate);
        Assert.That($"CN={issuerDN}", Is.EqualTo(certificate.IssuerDN.Value));
        Assert.That(serialNumber, Is.EqualTo(certificate.SerialNumber.Value));
        Assert.That(notBefore, Is.EqualTo(certificate.NotBefore));
        Assert.That(notAfter, Is.EqualTo(certificate.NotAfter));
        Assert.That($"CN={subjectDN}", Is.EqualTo(certificate.SubjectDN.Value));
        Assert.That(signatureAlgorithm, Is.EqualTo(certificate.SignatureAlgorithm.Value));
    }

    [Test]
    public void CreateCertificate_WithIdentifier_UsingBuilder_ReturnsValidCertificate()
    {
        // Arrange
        var builder = new CertificateBuilder(_certificateFactory);
        var id = Guid.NewGuid();
        var issuerDN = "Issuer";
        var serialNumber = "123456789";
        var notBefore = DateTime.UtcNow;
        var notAfter = DateTime.UtcNow.AddYears(1);
        var subjectDN = "Subject";
        var signatureAlgorithm = SignatureAlgorithmEnum.Sha256WithRsa;

        // Act
        var certificate = builder.WithIdentifier(id)
                                 .WithIssuerDN(issuerDN)
                                 .WithSerialNumber(serialNumber)
                                 .WithNotBefore(notBefore)
                                 .WithNotAfter(notAfter)
                                 .WithSubjectDN(subjectDN)
                                 .WithPublicKey(_keyPair.PublicKey.Value)
                                 .WithSignatureAlgorithm(signatureAlgorithm)
                                 .Build();

        // Assert
        Assert.IsNotNull(certificate);
        Assert.That(id, Is.EqualTo(certificate.Id));
        Assert.That($"CN={issuerDN}", Is.EqualTo(certificate.IssuerDN.Value));
        Assert.That(serialNumber, Is.EqualTo(certificate.SerialNumber.Value));
        Assert.That(notBefore, Is.EqualTo(certificate.NotBefore));
        Assert.That(notAfter, Is.EqualTo(certificate.NotAfter));
        Assert.That($"CN={subjectDN}", Is.EqualTo(certificate.SubjectDN.Value));
        Assert.That(signatureAlgorithm, Is.EqualTo(certificate.SignatureAlgorithm.Value));
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
        var signatureAlgorithm = SignatureAlgorithmEnum.Sha256WithRsa;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            factory.Factory(issuerDN, serialNumber, notBefore, notAfter, subjectDN, publicKey, signatureAlgorithm));
    }
}