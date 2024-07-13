using System.Text;
using CertificationAuthority.Application.UseCases.CreateCertificate;
using CertificationAuthority.Domain.Builders;
using CertificationAuthority.Domain.Enumerations;
using CertificationAuthority.Domain.Factories;
using CertificationAuthority.Infrastructure;
using Moq;

namespace CertificationAuthority.Test.UnitTest;

public class CreateCertificateHandlerTests
{
    private Mock<ICertificateGenerator> _mockCertificateGenerator;
    private Mock<ICertificateBuilder> _mockCertificateBuilder;
    private CreateCertificateHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mockCertificateGenerator = new Mock<ICertificateGenerator>();
        _mockCertificateBuilder = new Mock<ICertificateBuilder>();
        _handler = new CreateCertificateHandler(_mockCertificateGenerator.Object, _mockCertificateBuilder.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnCertificateResponse()
    {
        // Arrange
        var request = new CreateCertificateRequest(
            IssuerDN: "CN=Issuer",
            SerialNumber: "123456",
            SubjectDN: "CN=Subject",
            NotBefore: DateTime.UtcNow,
            NotAfter: DateTime.UtcNow.AddYears(1),
            PublicKey: "publicKey",
            SignatureAlgorithm: SignatureAlgorithmEnum.SHA256WithRSA,
            SenderPrivateKey: "privateKey"
        );

        var domainCertificate = new PKICertificateFactory()
            .Factory(
                request.IssuerDN,
                request.SerialNumber,
                request.NotBefore,
                request.NotAfter,
                request.SubjectDN,
                request.PublicKey,
                request.SignatureAlgorithm);

        var expectedCertificate = new byte[] { 1, 2, 3, 4, 5 };

        _mockCertificateBuilder.Setup(cb => cb
            .WithIssuerDN(It.IsAny<string>())
            .WithSerialNumber(It.IsAny<string>())
            .WithSubjectDN(It.IsAny<string>())
            .WithNotBefore(It.IsAny<DateTime>())
            .WithNotAfter(It.IsAny<DateTime>())
            .WithPublicKey(It.IsAny<string>())
            .WithSignatureAlgorithm(It.IsAny<SignatureAlgorithmEnum>())
            .Build())
            .Returns(domainCertificate);

        var senderPrivateKey = Encoding.UTF8.GetBytes(request.SenderPrivateKey);

        _mockCertificateGenerator
            .Setup(cg => cg.X509CreateCertificate(domainCertificate, senderPrivateKey))
            .Returns(expectedCertificate);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsNotNull(response);
        Assert.That(expectedCertificate, Is.EqualTo(response.Certificate));
    }

    [Test]
    public void Handle_ShouldThrowException_WhenBuilderFails()
    {
        // Arrange
        var request = new CreateCertificateRequest
            (
                IssuerDN: "CN=Issuer",
                SerialNumber: "123456",
                NotBefore: DateTime.UtcNow,
                NotAfter: DateTime.UtcNow.AddYears(1),
                SubjectDN: "CN=Subject",
                PublicKey: "publicKey",
                SenderPrivateKey: "privateKey",
                SignatureAlgorithm: SignatureAlgorithmEnum.SHA256WithRSA
            );

        _mockCertificateBuilder.Setup(cb => cb
            .WithIssuerDN(It.IsAny<string>())
            .WithSerialNumber(It.IsAny<string>())
            .WithSubjectDN(It.IsAny<string>())
            .WithNotBefore(It.IsAny<DateTime>())
            .WithNotAfter(It.IsAny<DateTime>())
            .WithPublicKey(It.IsAny<string>())
            .WithSignatureAlgorithm(It.IsAny<SignatureAlgorithmEnum>())
            .Build())
            .Throws(new InvalidOperationException("Builder failed"));

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () => await _handler.Handle(request, CancellationToken.None));
    }
}
