using SupportChat.Application.Dtos;

namespace SupportChat.Application.Interfaces;

public interface ISessionManager
{
    int CreateSession();
    SessionStatusDto CheckSession(int id);
    int? GetNextUnassignedSessionId();
    void Requeue(int id);
}