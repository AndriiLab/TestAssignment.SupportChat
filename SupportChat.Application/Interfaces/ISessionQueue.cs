using SupportChat.Core.Queue;

namespace SupportChat.Application.Interfaces;

public interface ISessionQueue : IQueue<int>
{
    int Size { get; }
    void SetSize(int newSize);
}