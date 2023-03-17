using System.Collections.Concurrent;
using SupportChat.Application.Interfaces;

namespace SupportChat.Infrastructure.Queues;

public class AgentQueue : IJuniorAgentQueue, IOverflowAgentQueue, IMiddleAgentQueue, ISeniorAgentQueue, ITeamLeadAgentQueue
{
    private readonly ConcurrentQueue<int> _queue;
    public AgentQueue()
    {
        _queue = new ConcurrentQueue<int>();
    }

    public bool Enqueue(int item)
    {
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
}