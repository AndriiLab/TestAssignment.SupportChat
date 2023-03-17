using SupportChat.Domain;

namespace SupportChat.Application.Interfaces;

public interface IDatabase
{
    IQueryable<Agent> Agents { get; set; }
    IQueryable<AgentTeam> AgentTeams { get; set; }
    IQueryable<Session> Sessions { get; set; }
    IQueryable<TeamShift> TeamSchedules { get; set; }
}