using DotNetCore.CAP;
using FluentScheduler;
using SupportChat.Application.Events;
using SupportChat.Application.Interfaces;

namespace SupportChat.Application.Background;

public class SessionInvalidatorJob : IJob
{
    private const int ExpiredAfterSecondsElapsed = 3; //TODO: extract to config

    private readonly IDatabase _database;
    private readonly ICapPublisher _bus;

    public SessionInvalidatorJob(IDatabase database, ICapPublisher bus)
    {
        _database = database;
        _bus = bus;
    }
    
    public void Execute()
    {
        var expiredDate = DateTime.Now.AddSeconds(-1 * ExpiredAfterSecondsElapsed);
        var expiredSessions = _database.Sessions.Where(s => s.IsActive && s.PingDate < expiredDate).ToArray();

        foreach (var session in expiredSessions)
        {
            session.IsActive = false;
            _bus.Publish(SessionInactivatedEvent.Key, new SessionInactivatedEvent(session.Id, session.AgentId));
        }
    }
}