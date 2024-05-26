namespace BarbarianGymStatistics;

public readonly struct TimeSpanRange
{
    public TimeSpan Start { get; }
    public TimeSpan End { get; }

    public TimeSpanRange(TimeSpan start, TimeSpan end)
    {
        if (start > end)
            throw new ArgumentException("Start time must be less than or equal to end time.");
        Start = start;
        End = end;
    }

    public bool Contains(TimeSpan time) => time >= Start && time <= End;

    public override string ToString() => $"[{Start} - {End}]";
}
