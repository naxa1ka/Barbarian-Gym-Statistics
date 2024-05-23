namespace BarbarianGymStatistics;

public interface IGymAvailability
{
    int LiveCount { get; }
    int Capacity { get; }
}