using SupportChat.Domain;

namespace SupportChat.Application.Services.Scheduler;

public class TeamSchedule
{
    private List<DateTime> _shiftDates;
    public int Id { get; set; }
    public TeamType TeamType { get; set; }
    
    public TeamSchedule(int id, TeamType teamType, IEnumerable<DateTime> shiftDates)
    {
        _shiftDates = shiftDates.OrderBy(d => d).TakeLast(ScheduleConstants.ShiftDaysDuration).ToList();
        Id = id;
        TeamType = teamType;
    }
    
    public void AddLastShiftEnd(DateTime date)
    {
        _shiftDates.Add(date);
        _shiftDates = _shiftDates.OrderBy(d => d).TakeLast(ScheduleConstants.ShiftDaysDuration).ToList();
    }

    public bool CanBeScheduledOn(DateTime date, out (DateTime, DateTime) range)
    {
        range = (DateTime.MinValue, DateTime.MinValue);

        if (!ScheduleConstants.TeamTypeSchedule.TryGetValue(TeamType, out var weekSchedule))
        {
            return false;
        }

        if (!weekSchedule.TryGetValue(date.DayOfWeek, out var dayInterval))
        {
            return false;
        }
        
        var doCheckConsecutiveDays = _shiftDates.Count == ScheduleConstants.ShiftDaysDuration;
        if (doCheckConsecutiveDays)
        {
            var lastShiftEnd = _shiftDates.Last().Date;
            var daysDelta = (lastShiftEnd - _shiftDates.First().Date).Days;
            var hasAllConsecutiveDays = Enumerable.Range(1, daysDelta)
                .Select(i => lastShiftEnd.AddDays(-1 * i))
                .Where(expectedDay => weekSchedule.ContainsKey(expectedDay.DayOfWeek))
                .All(expectedDay => _shiftDates.Any(d => d.Date == expectedDay));

            if (hasAllConsecutiveDays)
            {
                return false;
            }
        }

        range = (date.Date.Add(dayInterval.Start), date.Date.Add(dayInterval.End));
        return true;
    }
}