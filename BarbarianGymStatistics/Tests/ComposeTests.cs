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
    private IGymAvailabilityService _gymAvailabilityService = null!;
    private IJournal _journal = null!;
    private Compose _compose = null!;
    private IDateTimeProvider _dateTimeProvider = null!;

    [SetUp]
    public void SetUp()
    {
        _timer = Substitute.For<ITimer>();
        _elapsedSubject = new Subject<Unit>();
        _timer.Elapsed.Returns(_elapsedSubject);
        _gymAvailabilityService = Substitute.For<IGymAvailabilityService>();
        _journal = Substitute.For<IJournal>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        _dateTimeProvider.Now.Returns(new DateTime(2007, 7, 7, 15, 0, 0));
        var workingHours = new TimeSpanRange(TimeSpan.FromHours(6), TimeSpan.FromHours(21));
        _compose = new Compose(
            _timer,
            _gymAvailabilityService,
            _journal,
            _dateTimeProvider,
            workingHours
        );
    }

    [Test]
    public async Task WhenTimerElapsed_ThenShouldGetGymAvailability()
    {
        // Arrange
        _compose.Start();
        _gymAvailabilityService.ClearReceivedCalls();

        // Act
        _elapsedSubject.OnNext(Unit.Default);
        await WaitForTheAsyncMethodCall();

        // Assert
        await _gymAvailabilityService.Received(1).GetGymAvailabilityAsync();
    }

    [Test]
    public async Task WhenGymAvailabilityServiceThrowsException_ThenWriteExceptionInJournal()
    {
        // Arrange
        var exception = new Exception("Service Error");
        _gymAvailabilityService
            .GetGymAvailabilityAsync()
            .Returns(Task.FromException<GymAvailability>(exception));
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
        _gymAvailabilityService.GetGymAvailabilityAsync().Returns(Task.FromResult(gymAvailability));
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
        _gymAvailabilityService.GetGymAvailabilityAsync().Returns(Task.FromResult(gymAvailability));
        _timer.Elapsed.Returns(Observable.Empty<Unit>());

        // Act
        _compose.Start();
        await WaitForTheAsyncMethodCall();

        // Assert
        _journal.Received(1).Write(gymAvailability);
    }

    [Test]
    public async Task WhenStart_AndGymIsOpen_ThenShouldWriteGymAvailability()
    {
        // Arrange
        var gymAvailability = new GymAvailability(42, 666, false);
        _gymAvailabilityService.GetGymAvailabilityAsync().Returns(Task.FromResult(gymAvailability));
        _timer.Elapsed.Returns(Observable.Empty<Unit>());

        // Act
        _compose.Start();
        await WaitForTheAsyncMethodCall();

        // Assert
        _journal.Received(1).Write(gymAvailability);
    }

    [Test]
    public async Task WhenStart_AndGymIsClosed_ThenShouldDontWriteGymAvailability()
    {
        // Arrange
        var gymAvailability = new GymAvailability(42, 666, true);
        _dateTimeProvider.Now.Returns(new DateTime(2007, 7, 7, 22, 0, 0));
        _gymAvailabilityService.GetGymAvailabilityAsync().Returns(Task.FromResult(gymAvailability));
        _timer.Elapsed.Returns(Observable.Empty<Unit>());

        // Act
        _compose.Start();
        await WaitForTheAsyncMethodCall();

        // Assert
        _journal.DidNotReceive().Write(gymAvailability);
    }

    private static Task WaitForTheAsyncMethodCall() => Task.Delay(TimeSpan.FromMilliseconds(100));
}
