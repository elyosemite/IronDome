using CertificationAuthority.Application.UseCases.CreatePublicPrivateKeyPair;
using CertificationAuthority.Infrastructure;
using Moq;

namespace CertificationAuthority.Test;

public class CreatePublicPrivateKeyPairHandlerTests
{
    private Mock<IPublicKeyPairGenerator> _mockPublicKeyPairGenerator;
    private CreatePublicPrivateKeyPairHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mockPublicKeyPairGenerator = new Mock<IPublicKeyPairGenerator>();
        _handler = new CreatePublicPrivateKeyPairHandler(_mockPublicKeyPairGenerator.Object);
    }

    [Test]
    public async Task Handle_ShouldReturnPublicPrivateKeyPairResponse()
    {
        // Arrange
        var expectedPublicKey = new byte[] { 1, 2, 3 };
        var expectedPrivateKey = new byte[] { 4, 5, 6 };
        var keyPair = new PublicKeyPair(expectedPublicKey, expectedPrivateKey);

        _mockPublicKeyPairGenerator
            .Setup(x => x.Generate())
            .Returns(keyPair);

        var request = new CreatePublicPrivateKeyPairRequest();

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(expectedPublicKey, Is.EqualTo(response.PublicKey));
        Assert.That(expectedPrivateKey, Is.EqualTo(response.PrivateKey));
    }
}
