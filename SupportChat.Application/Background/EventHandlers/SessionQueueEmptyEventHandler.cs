using DotNetCore.CAP;
using SupportChat.Application.Events;
using SupportChat.Application.Interfaces;
using SupportChat.Core.Events;

namespace SupportChat.Application.Background.EventHandlers;

public class SessionQueueEmptyEventHandler : IEventHandler<SessionQueueEmptyEvent>
{
    private readonly IAgentCoordinator _agentCoordinator;

    public SessionQueueEmptyEventHandler(IAgentCoordinator agentCoordinator)
    {
        _agentCoordinator = agentCoordinator;
    }
    
    [CapSubscribe(SessionQueueEmptyEvent.Key)]
    public void Handle(SessionQueueEmptyEvent @event)
    {
        _agentCoordinator.EmptyOverflowQueue();
    }
}