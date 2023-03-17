using SupportChat.Application.Interfaces;
using SupportChat.Domain;

namespace SupportChat.Application.Services.Scheduler;

public class TeamScheduler : ITeamScheduler
{
    private readonly IDatabase _database;

    public TeamScheduler(IDatabase database)
    {
        _database = database;
    }

    public bool CanAgentBeRescheduled(int agentId)
    {
        var now = DateTime.Now;
        return _database.TeamSchedules
            .Any(s => s.Start <= now && s.End >= now && s.AgentTeam.Agents.Any(a => a.Id == agentId));
    }

    public Agent[] GetActiveAgentsOfType(params TeamType[] types)
    {
        var now = DateTime.Now;
        var agents = _database.TeamSchedules
            .Where(s => s.Start <= now && s.End >= now && types.Contains(s.AgentTeam.Type))
            .SelectMany(s => s.AgentTeam.Agents)
            .ToArray();

        return agents;
    }

    public void CreateScheduleForMonth(int year, int month)
    {
        var teamSchedules = _database.AgentTeams
            .Select(t => new TeamSchedule(t.Id, t.Type,
                t.Shifts.Select(s => s.End).OrderBy(d => d).TakeLast(ScheduleConstants.ShiftDaysDuration)))
            .ToArray();

        foreach (var day in Enumerable.Range(1, DateTime.DaysInMonth(year, month))
                     .Select(d => new DateTime(year, month, d)))
        {
            var isRegularScheduled = false;
            var isNightShiftScheduled = false;
            var isOverflowScheduled = false;

            foreach (var teamSchedule in teamSchedules)
            {
                if (isRegularScheduled && isNightShiftScheduled && isOverflowScheduled)
                {
                    break;
                }

                if (!teamSchedule.CanBeScheduledOn(day, out var workingRange))
                {
                    continue;
                }

                _database.TeamSchedules.Append(new TeamShift
                {
                    AgentTeamId = teamSchedule.Id,
                    Start = workingRange.Item1,
                    End = workingRange.Item2
                });

                teamSchedule.AddLastShiftEnd(workingRange.Item2);
                switch (teamSchedule.TeamType)
                {
                    case TeamType.Regular:
                        isRegularScheduled = true;
                        break;
                    case TeamType.NightShift:
                        isNightShiftScheduled = true;
                        break;
                    case TeamType.Overflow:
                        isOverflowScheduled = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}