using CertificationAuthority.Application.UseCases.CreateCertificate;
using CertificationAuthority.Domain.Builders;
using Moq;

namespace CertificationAuthority.Test.UnitTest;

public class CreateCertificateHandlerTests
{
    private readonly Mock<ICertificateBuilder> _mockCertificateBuilder = new();
    private readonly CreateCertificateHandler _handler;

    public CreateCertificateHandlerTests()
    {
        _handler = new(_mockCertificateBuilder.Object);
    }
}
