namespace BarbarianGymStatistics;

public class ConsoleJournal : IJournal
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public ConsoleJournal(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public void Write(GymAvailability gymAvailability)
    {
        Console.WriteLine($"[{_dateTimeProvider.Now}] Current gym availability: {gymAvailability}");
    }

    public void Write(Exception exception)
    {
        Console.WriteLine($"[{_dateTimeProvider.Now}] An exception was thrown: {exception}");
    }
}
