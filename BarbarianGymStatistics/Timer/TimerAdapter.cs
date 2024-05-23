using System.Reactive;
using System.Reactive.Linq;
using System.Timers;
using Timer = System.Timers.Timer;

namespace BarbarianGymStatistics
{
    public class TimerAdapter : ITimer, IDisposable
    {
        private readonly Timer _timer;

        public IObservable<Unit> Elapsed =>
            Observable.FromEvent<ElapsedEventHandler, Unit>(
                handler => (_, _) => handler(Unit.Default),
                handler => _timer.Elapsed += handler,
                handler => _timer.Elapsed -= handler
            );

        public TimerAdapter(TimeSpan interval, bool autoReset)
        {
            _timer = new Timer(interval);
            _timer.AutoReset = autoReset;
        }

        public void Start() => _timer.Start();

        public void Stop() => _timer.Stop();

        public void Dispose() => _timer.Dispose();
    }
}
