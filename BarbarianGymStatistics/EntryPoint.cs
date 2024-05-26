using RestSharp;

namespace BarbarianGymStatistics;

public class EntryPoint
{
    public static void Main(string[] args)
    {
        var timerAdapter = new TimerAdapter(interval: TimeSpan.FromMinutes(30), autoReset: true);
        var options = new RestClientOptions("http://barbarian.myftp.org:1515")
        {
            UserAgent = "My Fitness Trainer",
        };
        var restClient = new RestClient(options);
        var restClientAdapter = new RestClientAdapter(restClient);
        var gymAvailabilityService = new Gym(restClientAdapter);
        const string filePath = "logs/logs.txt";
        Console.WriteLine($"Full path: {Path.GetFullPath(filePath)}");
        var dateTimeProvider = new SystemDateTimeProvider();
        var diskJournal = new DiskJournal(dateTimeProvider, filePath);
        var consoleJournal = new ConsoleJournal(dateTimeProvider);
        var journal = new CompositeJournal(new List<IJournal>() { diskJournal, consoleJournal });
        var root = new Compose(timerAdapter, gymAvailabilityService, journal);
        root.Start();
        Console.WriteLine("Program has been started");
        Console.WriteLine("Press any button to exit");
        Console.ReadLine();
        root.Dispose();
        timerAdapter.Dispose();
        restClient.Dispose();
        Console.WriteLine("Program has been stopped");
    }
}
