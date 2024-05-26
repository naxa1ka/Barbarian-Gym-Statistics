using FluentAssertions;
using NUnit.Framework;

namespace BarbarianGymStatistics.Tests;

public class GymAvailabilityTests
{
    [Test]
    [TestCase(10, 50)]
    [TestCase(15, 50)]
    [TestCase(10, 42)]
    public void WhenCreateGymAvailabilityFromXml_AndXmlIsValid_ThenShouldReturnExpectedValues(
        int liveCount,
        int capacity
    )
    {
        // Arrange
        var xmlResponse = GymAvailabilityTestDataFactory.CreateXmlResponse(liveCount, capacity);

        // Act
        var gymAvailability = GymAvailability.FromXml(xmlResponse);

        // Assert
        gymAvailability.LiveCount.Should().Be(liveCount);
        gymAvailability.Capacity.Should().Be(capacity);
        gymAvailability.IsClosed.Should().BeFalse();
    }

    [Test]
    public void WhenCreateGymAvailabilityFromXml_AndXmlIsInvalid_ThenShouldThrowException()
    {
        // Arrange
        const string invalidXmlResponse = @"<invalidXml></invalidXml>";

        // Act
        Action create = () => GymAvailability.FromXml(invalidXmlResponse);

        // Assert
        create.Should().Throw<Exception>();
    }

    [Test]
    public void WhenCreateGymAvailabilityFromXml_AndXmlWhenGymIsClosed_ThenIsClosedShouldBeTrue()
    {
        // Arrange
        var xmlResponse = GymAvailabilityTestDataFactory.CreateXmlResponseWhenGymIsClosed();

        // Act
        var gymAvailability = GymAvailability.FromXml(xmlResponse);

        // Assert
        gymAvailability.IsClosed.Should().BeTrue();
        gymAvailability.LiveCount.Should().Be(0);
        gymAvailability.Capacity.Should().Be(0);
    }
}
