using System.Collections.Concurrent;
using SupportChat.Application.Interfaces;

namespace SupportChat.Infrastructure.Queues;

public class SessionQueue : ISessionQueue
{
    private readonly ConcurrentQueue<int> _queue;
    public int Size { get; private set; }

    public SessionQueue()
    {
        _queue = new ConcurrentQueue<int>();
        Size = 0;
    }

    public bool Enqueue(int item)
    {
        if (_queue.Count >= Size)
        {
            return false;
        }

        _queue.Enqueue(item);
        return true;
    }

    public bool TryDequeue(out int item)
    {
        return _queue.TryDequeue(out item);
    }

    public bool TryPeek(out int item)
    {
        return _queue.TryPeek(out item);
    }

    public void Clear()
    {
        _queue.Clear();
    }

    public void SetSize(int newSize)
    {
        Size = newSize;
    }
}