using BarbarianGymStatistics;

public class Program
{
    public static void Main(string[] args)
    {
        var timerAdapter = new TimerAdapter(
            interval: TimeSpan.FromMinutes(30),
            autoReset: true
        );
        var httpClientAdapter = new HttpClientAdapter();
        var gymAvailabilityService = new GymAvailabilityService(
            httpClientAdapter
        );
        var diskJournal = new DiskJournal();
        var root = new Root(
            timerAdapter,
            gymAvailabilityService,
            diskJournal
        );
        root.Start();
        Console.WriteLine("Program has been started");
        Console.WriteLine("Press any button to exit");
        Console.ReadLine();
        root.Dispose();
        timerAdapter.Dispose();
        httpClientAdapter.Dispose();    
        Console.WriteLine("Program has been stopped");
    }
    
}
