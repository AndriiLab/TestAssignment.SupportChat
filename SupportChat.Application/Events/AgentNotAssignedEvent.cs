using SupportChat.Core.Events;

namespace SupportChat.Application.Events;

public class AgentNotAssignedEvent : IEvent
{
    public const string Key = "AgentNotAssigned";

    public AgentNotAssignedEvent(int agentId)
    {
        AgentId = agentId;
    }


    public int AgentId { get; }
}