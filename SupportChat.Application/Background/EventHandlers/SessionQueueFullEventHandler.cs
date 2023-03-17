using DotNetCore.CAP;
using SupportChat.Application.Events;
using SupportChat.Application.Interfaces;
using SupportChat.Core.Events;

namespace SupportChat.Application.Background.EventHandlers;

public class SessionQueueFullEventHandler : IEventHandler<SessionQueueFullEvent>
{
    private readonly IAgentCoordinator _agentCoordinator;

    public SessionQueueFullEventHandler(IAgentCoordinator agentCoordinator)
    {
        _agentCoordinator = agentCoordinator;
    }
    
    [CapSubscribe(SessionQueueFullEvent.Key)]
    public void Handle(SessionQueueFullEvent @event)
    {
        _agentCoordinator.EnqueueOverflowAgents();
    }
}