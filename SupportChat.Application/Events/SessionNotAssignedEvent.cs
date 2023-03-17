using SupportChat.Core.Events;

namespace SupportChat.Application.Events;

public class SessionNotAssignedEvent : IEvent
{
    public const string Key = "SessionNotAssigned";

    public SessionNotAssignedEvent(int sessionId)
    {
        SessionId = sessionId;
    }


    public int SessionId { get; }
}