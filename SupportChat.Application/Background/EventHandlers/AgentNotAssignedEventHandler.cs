using DotNetCore.CAP;
using SupportChat.Application.Events;
using SupportChat.Application.Interfaces;
using SupportChat.Core.Events;

namespace SupportChat.Application.Background.EventHandlers;

public class AgentNotAssignedEventHandler : IEventHandler<AgentNotAssignedEvent>
{
    private readonly IAgentCoordinator _agentCoordinator;

    public AgentNotAssignedEventHandler(IAgentCoordinator agentCoordinator)
    {
        _agentCoordinator = agentCoordinator;
    }
    
    [CapSubscribe(AgentNotAssignedEvent.Key)]
    public void Handle(AgentNotAssignedEvent @event)
    {
        _agentCoordinator.RequeueAgent(@event.AgentId);
    }
}