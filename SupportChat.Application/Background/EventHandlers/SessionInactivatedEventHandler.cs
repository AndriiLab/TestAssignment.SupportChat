using DotNetCore.CAP;
using SupportChat.Application.Events;
using SupportChat.Application.Interfaces;
using SupportChat.Core.Events;

namespace SupportChat.Application.Background.EventHandlers;

public class SessionInactivatedEventHandler : IEventHandler<SessionInactivatedEvent>
{
    private readonly IAgentCoordinator _coordinator;

    public SessionInactivatedEventHandler(IAgentCoordinator coordinator)
    {
        _coordinator = coordinator;
    }

    [CapSubscribe(SessionInactivatedEvent.Key)]
    public void Handle(SessionInactivatedEvent @event)
    {
        if (@event.AgentId.HasValue)
        {
            _coordinator.EnqueueAgent(@event.AgentId.Value);
        }
    }
}