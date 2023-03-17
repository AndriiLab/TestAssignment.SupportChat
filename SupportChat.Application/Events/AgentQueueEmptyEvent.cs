using SupportChat.Core.Events;

namespace SupportChat.Application.Events;

public class AgentQueueEmptyEvent : IEvent
{
    public const string Key = "AgentQueueEmptyEvent";

}