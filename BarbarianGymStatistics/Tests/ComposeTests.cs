using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using NSubstitute;
using NUnit.Framework;

namespace BarbarianGymStatistics.Tests;

public class ComposeTests
{
    private ITimer _timer = null!;
    private Subject<Unit> _elapsedSubject = null!;
    private IGym _gym = null!;
    private IJournal _journal = null!;
    private Compose _compose = null!;

    [SetUp]
    public void SetUp()
    {
        _timer = Substitute.For<ITimer>();
        _elapsedSubject = new Subject<Unit>();
        _timer.Elapsed.Returns(_elapsedSubject);
        _gym = Substitute.For<IGym>();
        _journal = Substitute.For<IJournal>();
        _compose = new Compose(_timer, _gym, _journal);
    }

    [Test]
    public async Task WhenTimerElapsed_ThenShouldGetGymAvailability()
    {
        // Arrange
        _compose.Start();
        _gym.ClearReceivedCalls();

        // Act
        _elapsedSubject.OnNext(Unit.Default);
        await WaitForTheAsyncMethodCall();

        // Assert
        await _gym.Received(1).GetGymAvailabilityAsync();
    }

    [Test]
    public async Task WhenGymAvailabilityServiceThrowsException_ThenWriteExceptionInJournal()
    {
        // Arrange
        var exception = new Exception("Service Error");
        _gym.GetGymAvailabilityAsync().Returns(Task.FromException<GymAvailability>(exception));
        _timer.Elapsed.Returns(Observable.Empty<Unit>());

        // Act
        _compose.Start();
        await WaitForTheAsyncMethodCall();

        // Assert
        _journal.Received(1).Write(exception);
    }

    [Test]
    public async Task WhenTimerElapsed_ThenShouldWriteGymAvailability()
    {
        // Arrange
        var gymAvailability = new GymAvailability(42, 666, false);
        _gym.GetGymAvailabilityAsync().Returns(Task.FromResult(gymAvailability));
        _compose.Start();
        await WaitForTheAsyncMethodCall();
        _journal.ClearReceivedCalls();

        // Act
        _elapsedSubject.OnNext(Unit.Default);
        await WaitForTheAsyncMethodCall();

        // Assert
        _journal.Received(1).Write(gymAvailability);
    }

    [Test]
    public async Task WhenStart_ThenShouldWriteGymAvailabilityImmediately()
    {
        // Arrange
        var gymAvailability = new GymAvailability(42, 666, false);
        _gym.GetGymAvailabilityAsync().Returns(Task.FromResult(gymAvailability));
        _timer.Elapsed.Returns(Observable.Empty<Unit>());

        // Act
        _compose.Start();
        await WaitForTheAsyncMethodCall();

        // Assert
        _journal.Received(1).Write(gymAvailability);
    }

    private static Task WaitForTheAsyncMethodCall() => Task.Delay(TimeSpan.FromMilliseconds(100));
}
