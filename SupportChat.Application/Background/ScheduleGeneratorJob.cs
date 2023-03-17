using FluentScheduler;
using SupportChat.Application.Interfaces;
using SupportChat.Core.Extensions;

namespace SupportChat.Application.Background;

public abstract class ScheduleGeneratorJob : IJob
{
    private readonly int _year;
    private readonly int _month;
    private readonly IDatabase _database;
    private readonly ITeamScheduler _scheduler;

    protected ScheduleGeneratorJob(int year, int month, IDatabase database, ITeamScheduler scheduler)
    {
        _year = year;
        _month = month;
        _database = database;
        _scheduler = scheduler;
    }

    public void Execute()
    {
        var startOfMonth = new DateTime(_year, _month, 1);
        var startOfNextMonth = startOfMonth.AddMonths(1);

        if (_database.TeamSchedules.Any(s => s.Start >= startOfMonth && s.End <= startOfNextMonth))
        {
            return;
        }

        _scheduler.CreateScheduleForMonth(_year, _month);
    }
}

public class ScheduleGeneratorStartupJob : ScheduleGeneratorJob
{
    public ScheduleGeneratorStartupJob(IDatabase database, ITeamScheduler scheduler)
        : base(DateTime.Now.Year, DateTime.Now.Month, database, scheduler)
    {
    }
}

public class ScheduleGeneratorMonthlyJob : ScheduleGeneratorJob
{
    public ScheduleGeneratorMonthlyJob(IDatabase database, ITeamScheduler scheduler)
        : base(DateTime.Now.ToNextMonthStart().Year, DateTime.Now.ToNextMonthStart().Month, database, scheduler)
    {
    }
}