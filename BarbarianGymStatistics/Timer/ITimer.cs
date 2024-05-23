using System.Reactive;

namespace BarbarianGymStatistics;

public interface ITimer
{
    void Start();
    void Stop();
    IObservable<Unit> Elapsed { get; }
}