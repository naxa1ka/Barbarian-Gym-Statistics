namespace BarbarianGymStatistics;

public class DiskJournal : IJournal
{
    private readonly string _filePath;

    public DiskJournal(string filePath)
    {
        _filePath = filePath;
        EnsureDirectoryExists();
    }

    public void Write(GymAvailability gymAvailability)
    {
        var logEntry = $"[{DateTime.Now}] {gymAvailability}";
        WriteToFile(logEntry);
    }

    public void Write(Exception exception)
    {
        var logEntry = $"[{DateTime.Now}] Exception - {exception.Message}";
        WriteToFile(logEntry);
    }

    private void EnsureDirectoryExists()
    {
        var directoryPath = Path.GetDirectoryName(_filePath);
        if (directoryPath == null)
            throw new ArgumentNullException(nameof(directoryPath));
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath);
    }

    private void WriteToFile(string logEntry)
    {
        try
        {
            using var fs = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.Write);
            using var sw = new StreamWriter(fs);
            sw.WriteLine(logEntry);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to write to log file: {ex.Message}");
        }
    }
}
