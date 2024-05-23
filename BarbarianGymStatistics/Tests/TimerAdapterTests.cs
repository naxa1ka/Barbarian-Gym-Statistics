using FluentAssertions;
using NUnit.Framework;

namespace BarbarianGymStatistics.Tests
{
    public class TimerAdapterTests
    {
        private TimerAdapter _timerAdapter = null!;
        private TimeSpan _interval;

        [SetUp]
        public void Setup()
        {
            _interval = TimeSpan.FromMilliseconds(100);
            _timerAdapter = new TimerAdapter(_interval, true);
        }

        [TearDown]
        public void TearDown()
        {
            _timerAdapter.Dispose();
        }

        [Test]
        public void WhenStart_AndIntervalPasses_ThenElapsedEventShouldBeRaisedOnce()
        {
            // Arrange
            var receivedEvents = 0;
            _timerAdapter.Elapsed.Subscribe(_ => receivedEvents++);

            // Act
            _timerAdapter.Start();
            ThreadSleepIntervalWithMargin();
            _timerAdapter.Stop();

            // Assert
            receivedEvents.Should().Be(1);
        }

        [Test]
        public void WhenStartAndDispose_AndIntervalPasses_ThenNoElapsedEventShouldBeRaised()
        {
            // Arrange
            var receivedEvents = 0;
            _timerAdapter.Elapsed.Subscribe(_ => receivedEvents++);

            // Act
            _timerAdapter.Start();
            _timerAdapter.Dispose();
            ThreadSleepIntervalWithMargin();

            // Assert
            receivedEvents.Should().Be(0);
        }

        [Test]
        public void WhenStartAndStop_AndIntervalPasses_ThenNoElapsedEventShouldBeRaisedAfterStop()
        {
            // Arrange
            var receivedEvents = 0;
            _timerAdapter.Elapsed.Subscribe(_ => receivedEvents++);

            // Act
            _timerAdapter.Start();
            ThreadSleepIntervalWithMargin();
            _timerAdapter.Stop();
            ThreadSleepIntervalWithMargin();

            // Assert
            receivedEvents.Should().Be(1);
        }

        [Test]
        public void WhenStartWithAutoReset_AndIntervalPassesTwice_ThenElapsedEventShouldBeRaisedTwice()
        {
            // Arrange
            var receivedEvents = 0;
            _timerAdapter.Elapsed.Subscribe(_ => receivedEvents++);

            // Act
            _timerAdapter.Start();
            ThreadSleepIntervalWithMargin();
            ThreadSleepIntervalWithMargin();
            _timerAdapter.Stop();

            // Assert
            receivedEvents.Should().Be(2);
        }

        private void ThreadSleepIntervalWithMargin() => Thread.Sleep(_interval + _interval / 5);
    }
}
