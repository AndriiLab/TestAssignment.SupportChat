using SupportChat.Core.Structures;
using SupportChat.Domain;

namespace SupportChat.Application.Services.Scheduler;

public static class ScheduleConstants
{
    public const int ShiftDaysDuration = 3;

    public static readonly IReadOnlyDictionary<TeamType, Dictionary<DayOfWeek, TimeInterval>> TeamTypeSchedule =
        new Dictionary<TeamType, Dictionary<DayOfWeek, TimeInterval>>()
        {
            {
                TeamType.Regular, new Dictionary<DayOfWeek, TimeInterval>
                {
                    { DayOfWeek.Monday, new(9, 16) },
                    { DayOfWeek.Tuesday, new(9, 16) },
                    { DayOfWeek.Wednesday, new(9, 16) },
                    { DayOfWeek.Thursday, new(9, 16) },
                    { DayOfWeek.Friday, new(9, 16) },
                }
            },
            {
                TeamType.NightShift, new Dictionary<DayOfWeek, TimeInterval>
                {
                    { DayOfWeek.Monday, new(16, 23) },
                    { DayOfWeek.Tuesday, new(16, 23) },
                    { DayOfWeek.Wednesday, new(16, 23) },
                    { DayOfWeek.Thursday, new(16, 23) },
                    { DayOfWeek.Friday, new(16, 23) },
                }
            },
            {
                TeamType.Overflow, new Dictionary<DayOfWeek, TimeInterval>
                {
                    { DayOfWeek.Monday, new(9, 16) },
                    { DayOfWeek.Tuesday, new(9, 16) },
                    { DayOfWeek.Wednesday, new(9, 16) },
                    { DayOfWeek.Thursday, new(9, 16) },
                    { DayOfWeek.Friday, new(9, 16) },
                }
            },
        };
}