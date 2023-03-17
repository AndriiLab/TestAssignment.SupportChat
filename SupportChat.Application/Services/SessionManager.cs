using DotNetCore.CAP;
using SupportChat.Application.Dtos;
using SupportChat.Application.Events;
using SupportChat.Application.Interfaces;
using SupportChat.Core.Exceptions;
using SupportChat.Domain;

namespace SupportChat.Application.Services;

public class SessionManager : ISessionManager
{
    private const double CapacityMultiplier = 1.5;

    private readonly IDatabase _database;
    private readonly ISessionQueue _sessionQueue;
    private readonly ICapPublisher _bus;
    private readonly IAgentCoordinator _agentCoordinator;

    public SessionManager(IDatabase database, ISessionQueue sessionQueue, ICapPublisher bus,
        IAgentCoordinator agentCoordinator)
    {
        _database = database;
        _sessionQueue = sessionQueue;
        _bus = bus;
        _agentCoordinator = agentCoordinator;
    }

    public int CreateSession()
    {
        var teamCapacity = _agentCoordinator.GetCurrentTeamCapacity();
        if (teamCapacity < 1)
        {
            throw new SupportChatException("Service available only within working hours and days");
        }

        var queueSize = (int)(teamCapacity * CapacityMultiplier);
        _sessionQueue.SetSize(queueSize);

        var session = new Session();
        _database.Sessions.Append(session);
        if (_sessionQueue.Enqueue(session.Id))
        {
            return session.Id;
        }

        _bus.Publish(SessionQueueFullEvent.Key, new SessionQueueFullEvent());
        throw new SupportChatException("Service overloaded");
    }

    public SessionStatusDto CheckSession(int id)
    {
        var session = _database.Sessions.FirstOrDefault(s => s.Id == id && s.IsActive);
        if (session is null)
        {
            throw new SupportChatException("Session closed");
        }

        return new SessionStatusDto { AgentDescription = session.Agent?.Skill.ToString() };
    }

    public int? GetNextUnassignedSessionId()
    {
        if (_sessionQueue.TryDequeue(out var id))
        {
            return id;
        }
        
        _bus.Publish(SessionQueueEmptyEvent.Key, new SessionQueueEmptyEvent());

        return  null;
    }

    public void Requeue(int id)
    {
        _sessionQueue.Enqueue(id); // TODO: should append to start of queue method
    }
}