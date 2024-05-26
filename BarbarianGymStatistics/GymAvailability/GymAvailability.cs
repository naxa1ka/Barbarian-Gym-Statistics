using System.Xml.Linq;

namespace BarbarianGymStatistics;

public class GymAvailability : IGymAvailability
{
    public int LiveCount { get; }
    public int Capacity { get; }
    public bool IsClosed { get; }

    public GymAvailability(int liveCount, int capacity, bool isClosed)
    {
        LiveCount = liveCount;
        Capacity = capacity;
        IsClosed = isClosed;
    }

    public static GymAvailability Closed => new(0, 0, true);

    public static GymAvailability FromXml(string xml)
    {
        const string columnName = "c";
        const string dataSetName = "DataSet";
        var doc = XDocument.Parse(xml);
        var dataSet = doc.Descendants(dataSetName).Single();
        var columns = dataSet.Descendants(columnName).ToList();
        try
        {
            var liveCount = int.Parse(columns.ElementAt(1).Value);
            var capacity = int.Parse(columns.ElementAt(2).Value);
            return new GymAvailability(liveCount, capacity, false);
        }
        catch (Exception exception)
        {
            if (exception is ArgumentOutOfRangeException)
                return Closed;
            throw;
        }
    }

    public override string ToString() => $"The gym is {IsClosedToString()}: {LiveCount}/{Capacity}";

    private string IsClosedToString() => IsClosed ? "closed" : "open";
}
