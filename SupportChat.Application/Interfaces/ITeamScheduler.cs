using SupportChat.Domain;

namespace SupportChat.Application.Interfaces;

public interface ITeamScheduler
{
    void CreateScheduleForMonth(int year, int month);
    bool CanAgentBeRescheduled(int agentId);
    Agent[] GetActiveAgentsOfType(params TeamType[] types);
}