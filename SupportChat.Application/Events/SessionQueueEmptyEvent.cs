using SupportChat.Core.Events;

namespace SupportChat.Application.Events;

public class SessionQueueEmptyEvent : IEvent
{
    public const string Key = "SessionQueueEmptyEvent";
}