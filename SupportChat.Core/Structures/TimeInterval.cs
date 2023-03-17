namespace SupportChat.Core.Structures;

public struct TimeInterval
{
    public TimeInterval(int hourStart, int hourEnd)
    {
        Start = new TimeSpan(hourStart, 0, 0);
        End = new TimeSpan(hourEnd, 0, 0);
    }

    public TimeInterval(TimeSpan start, TimeSpan end)
    {
        Start = start;
        End = end;
    }

    public TimeSpan Start { get; }
    public TimeSpan End { get; }
}