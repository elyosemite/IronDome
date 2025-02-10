using CertificationAuthority.Application.UseCases.CreateCertificate;
using CertificationAuthority.Domain.Builders;
using CertificationAuthority.Domain.Certificate;
using CertificationAuthority.Domain.Enumerations;
using CertificationAuthority.Domain.Factories;
using CertificationAuthority.Infrastructure;
using Moq;

namespace CertificationAuthority.Test.UnitTest;

[TestFixture]
public class CreateCertificateHandlerTests
{
    private readonly Mock<ICertificateBuilder> _certificateBuilderMock = new();

    public CreateCertificateHandlerTests()
    {

    }

    [Test]
    public async Task Add_WhenCalled_ReturnsSum()
    {
        // Arrange
        var ironDomepublicKeyPair = PublicKeyPairGenerator.Generate();
        var publicKeyPair = PublicKeyPairGenerator.Generate();

        CreateCertificateRequest request = new(
            IssuerDN: "CN=Issuer",
            SerialNumber: "123456",
            SubjectDN: "CN=Subject",
            NotBefore: DateTime.UtcNow,
            NotAfter: DateTime.UtcNow.AddYears(1),
            PublicKey: Convert.ToBase64String(publicKeyPair.publicKey),
            SignatureAlgorithm: SignatureAlgorithmEnum.SHA256WithRSA,
            SenderPrivateKey: Convert.ToBase64String(ironDomepublicKeyPair.privateKey)
        );

        PKICertificate domainCertificate = new PKICertificateFactory()
            .Factory(
                request.IssuerDN,
                request.SerialNumber,
                request.NotBefore,
                request.NotAfter,
                request.SubjectDN,
                request.PublicKey,
                request.SignatureAlgorithm);

        _certificateBuilderMock
            .Setup(o => o.Build())
            .Returns(domainCertificate);

        // Act
        var handler = new CreateCertificateHandler(_certificateBuilderMock.Object);
        var response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.IsNotNull(response);
        //Assert.That(expectedCertificate, Is.EqualTo(response.Certificate));
    }

    [Test]
    public async Task CreateCertificateHandler_CreateSuccessfullyCSRFile()
    {

    }

    [Test]
    public async Task CreateCertificate_FromCSRFile()
    {

    }
}
