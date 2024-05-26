using System.Reactive;
using System.Reactive.Disposables;

namespace BarbarianGymStatistics;

public class Compose : IDisposable
{
    private readonly ITimer _timer;
    private readonly IGymAvailabilityService _gymAvailabilityService;
    private readonly IJournal _journal;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly TimeSpanRange _workingHours;
    private IDisposable _subscriptionOnTimerElapsed = Disposable.Empty;

    public Compose(
        ITimer timer,
        IGymAvailabilityService gymAvailabilityService,
        IJournal journal,
        IDateTimeProvider dateTimeProvider,
        TimeSpanRange workingHours
    )
    {
        _timer = timer;
        _gymAvailabilityService = gymAvailabilityService;
        _journal = journal;
        _dateTimeProvider = dateTimeProvider;
        _workingHours = workingHours;
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
        if (!_workingHours.Contains(_dateTimeProvider.Now.TimeOfDay))
            return;

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
