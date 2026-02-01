using FluentAssertions;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using NUnit.Framework;

namespace Identity.Test.Unit;

[TestFixture]
public class SubjectTests
{
    [Test]
    public void Constructor_ShouldThrowException_WhenCommonNameIsMissing()
    {
        // Act
        Action act = () => new Subject(Guid.NewGuid(), "", "email@test.com", SubjectType.Person);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Common Name is required*");
    }

    [Test]
    public void Constructor_ShouldThrowException_WhenEmailIsMissing()
    {
        // Act
        Action act = () => new Subject(Guid.NewGuid(), "John Doe", "", SubjectType.Person);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Email is required*");
    }

    [Test]
    public void Constructor_ShouldCreateSubject_WhenValid()
    {
        // Arrange
        var orgId = Guid.NewGuid();
        var name = "Jane Doe";
        var email = "jane@test.com";

        // Act
        var subject = new Subject(orgId, name, email, SubjectType.Person, "IT");

        // Assert
        subject.Id.Should().NotBeEmpty();
        subject.OrganizationId.Should().Be(orgId);
        subject.CommonName.Should().Be(name);
        subject.Email.Should().Be(email);
        subject.Department.Should().Be("IT");
    }
}
