namespace SupportChat.Application.Interfaces;

public interface IAgentCoordinator
{
    int? GetNextAgentId();
    int GetCurrentTeamCapacity();
    void EnqueueAgent(int agentId);
    void RequeueAgent(int agentId);
    void EnqueueRegularAgents();
    void EnqueueOverflowAgents();
    void EmptyOverflowQueue();
}