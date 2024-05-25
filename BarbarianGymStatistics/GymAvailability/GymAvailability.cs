using System.Xml.Linq;

namespace BarbarianGymStatistics;

public class GymAvailability : IGymAvailability
{
    public int LiveCount { get; }
    public int Capacity { get; }

    public GymAvailability(int liveCount, int capacity)
    {
        LiveCount = liveCount;
        Capacity = capacity;
    }

    public static GymAvailability FromXml(string xml)
    {
        const string columnName = "c";
        const string dataSetName = "DataSet";
        var doc = XDocument.Parse(xml);
        var dataSet = doc.Descendants(dataSetName).Single();
        var columns = dataSet.Descendants(columnName).ToList();
        var liveCount = columns.ElementAt(1).Value;
        var capacity = columns.ElementAt(2).Value;
        return new GymAvailability(int.Parse(liveCount), int.Parse(capacity));
    }

    public override string ToString() => $"{LiveCount}/{Capacity}";
}
