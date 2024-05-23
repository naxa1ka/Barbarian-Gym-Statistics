namespace BarbarianGymStatistics;

public interface IJournal
{
    void Write(GymAvailability gymAvailability);
    void Write(Exception exception);
}
