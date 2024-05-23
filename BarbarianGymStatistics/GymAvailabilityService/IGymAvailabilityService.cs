namespace BarbarianGymStatistics;

public interface IGymAvailabilityService
{
    Task<GymAvailability> GetGymAvailabilityAsync(CancellationToken cancellationToken = default);
}
