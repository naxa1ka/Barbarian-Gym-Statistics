using System.Reactive;
using System.Reactive.Disposables;

namespace BarbarianGymStatistics;

public class Compose : IDisposable
{
    private readonly ITimer _timer;
    private readonly IGymAvailabilityService _gymAvailabilityService;
    private readonly IJournal _journal;
    private IDisposable _subscriptionOnTimerElapsed = Disposable.Empty;

    public Compose(ITimer timer, IGymAvailabilityService gymAvailabilityService, IJournal journal)
    {
        _timer = timer;
        _gymAvailabilityService = gymAvailabilityService;
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
            var gymAvailability = await _gymAvailabilityService.GetGymAvailabilityAsync();
            _journal.Write(gymAvailability);
        }
        catch (Exception exception)
        {
            _journal.Write(exception);
        }
    }

    public void Dispose() => _subscriptionOnTimerElapsed.Dispose();
}
