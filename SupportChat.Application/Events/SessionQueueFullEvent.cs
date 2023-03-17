using SupportChat.Core.Events;

namespace SupportChat.Application.Events;

public class SessionQueueFullEvent : IEvent
{
    public const string Key = "SessionQueueFull";
}