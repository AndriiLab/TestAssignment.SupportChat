using SupportChat.Core.Events;

namespace SupportChat.Application.Events;

public class SessionInactivatedEvent : IEvent
{
    public const string Key = "SessionInactivated";
    
    public SessionInactivatedEvent(int sessionId, int? agentId)
    {
        SessionId = sessionId;
        AgentId = agentId;
    }

    public int SessionId { get; }
    public int? AgentId { get; }
}