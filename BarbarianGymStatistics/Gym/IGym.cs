namespace BarbarianGymStatistics;

public interface IGym
{
    Task<GymAvailability> GetGymAvailabilityAsync(CancellationToken cancellationToken = default);
}
