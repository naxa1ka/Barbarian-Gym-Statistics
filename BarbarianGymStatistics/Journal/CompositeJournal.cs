namespace BarbarianGymStatistics;

public class CompositeJournal : IJournal
{
    private readonly List<IJournal> _journals;

    public CompositeJournal(IEnumerable<IJournal> journals)
    {
        _journals = journals.ToList();
    }

    public void Write(GymAvailability gymAvailability)
    {
        foreach (var journal in _journals)
            journal.Write(gymAvailability);
    }

    public void Write(Exception exception)
    {
        foreach (var journal in _journals)
            journal.Write(exception);
    }
}
