namespace BarbarianGymStatistics;

public class ConsoleJournal : IJournal
{
    public void Write(GymAvailability gymAvailability)
    {
        Console.WriteLine($"[{DateTime.Now}] Current gym availability: {gymAvailability}");
    }

    public void Write(Exception exception)
    {
        Console.WriteLine($"[{DateTime.Now}] An exception was thrown: {exception}");
    }
}
