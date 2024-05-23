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
}
