using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using RestSharp;

namespace BarbarianGymStatistics.Tests;

public class GymAvailabilityServiceTests
{
    [Test]
    public async Task WhenGetGymAvailabilityAsync_AndResponseIsSuccess_ThenShouldReturnGymAvailability()
    {
        // Arrange
        var client = Substitute.For<IRestClient>();
        const int liveCount = 10;
        const int capacity = 50;
        var response = new RestResponse
        {
            Content = GymAvailabilityTestDataFactory.CreateXmlResponse(liveCount, capacity),
            StatusCode = System.Net.HttpStatusCode.OK
        };
        client.ExecuteAsync(Arg.Any<RestRequest>()).Returns(Task.FromResult(response));

        var service = new GymAvailabilityService(client);

        // Act
        var result = await service.GetGymAvailabilityAsync();

        // Assert
        await client.Received(1).ExecuteAsync(Arg.Any<RestRequest>());
        result.LiveCount.Should().Be(liveCount);
        result.Capacity.Should().Be(capacity);
    }

    [Test]
    public async Task WhenGetGymAvailabilityAsync_AndResponseIsNotSuccess_ThenShouldThrowException()
    {
        // Arrange
        var client = Substitute.For<IRestClient>();
        var response = new RestResponse
        {
            Content = "<xml>Response content here</xml>",
            StatusCode = System.Net.HttpStatusCode.BadRequest
        };
        client.ExecuteAsync(Arg.Any<RestRequest>()).Returns(Task.FromResult(response));

        var service = new GymAvailabilityService(client);

        // Act
        Func<Task> act = async () => await service.GetGymAvailabilityAsync();

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
