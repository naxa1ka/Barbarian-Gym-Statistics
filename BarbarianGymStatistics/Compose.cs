using System.Reactive;
using System.Reactive.Disposables;

namespace BarbarianGymStatistics;

public class Compose : IDisposable
{
    private readonly ITimer _timer;
    private readonly IGym _gym;
    private readonly IJournal _journal;

    private IDisposable _subscriptionOnTimerElapsed = Disposable.Empty;

    public Compose(ITimer timer, IGym gym, IJournal journal)
    {
        _timer = timer;
        _gym = gym;
        _journal = journal;
    }

    public void Start()
    {
        _subscriptionOnTimerElapsed = _timer.Elapsed.Subscribe(OnTimerElapsed);
        _ = Task.Run(WriteGymAvailability);
        _timer.Start();
    }

    private async void OnTimerElapsed(Unit _) => await WriteGymAvailability();

    private async Task WriteGymAvailability()
    {
        try
        {
            var gymAvailability = await _gym.GetGymAvailabilityAsync();
            _journal.Write(gymAvailability);
        }
        catch (Exception exception)
        {
            _journal.Write(exception);
        }
    }

    public void Dispose() => _subscriptionOnTimerElapsed.Dispose();
}
