using DotNetCore.CAP;
using SupportChat.Application.Events;
using SupportChat.Application.Interfaces;
using SupportChat.Core.Events;

namespace SupportChat.Application.Background.EventHandlers;

public class AgentQueueEmptyEventHandler : IEventHandler<AgentQueueEmptyEvent>
{
    private readonly IAgentCoordinator _coordinator;

    public AgentQueueEmptyEventHandler(IAgentCoordinator coordinator)
    {
        _coordinator = coordinator;
    }
    
    [CapSubscribe(AgentQueueEmptyEvent.Key)]
    public void Handle(AgentQueueEmptyEvent @event)
    {
        _coordinator.EnqueueRegularAgents();
    }
}