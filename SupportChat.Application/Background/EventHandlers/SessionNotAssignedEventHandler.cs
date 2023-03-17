using DotNetCore.CAP;
using SupportChat.Application.Events;
using SupportChat.Application.Interfaces;
using SupportChat.Core.Events;

namespace SupportChat.Application.Background.EventHandlers;

public class SessionNotAssignedEventHandler : IEventHandler<SessionNotAssignedEvent>
{
    private readonly ISessionManager _sessionManager;

    public SessionNotAssignedEventHandler(ISessionManager sessionManager)
    {
        _sessionManager = sessionManager;
    }
    
    [CapSubscribe(SessionNotAssignedEvent.Key)]
    public void Handle(SessionNotAssignedEvent @event)
    {
        _sessionManager.Requeue(@event.SessionId);
    }
}