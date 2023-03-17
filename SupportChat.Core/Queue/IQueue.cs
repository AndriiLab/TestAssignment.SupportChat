namespace SupportChat.Core.Queue;

public interface IQueue<T>
{
    bool Enqueue(T item);
    bool TryDequeue(out T item);
    bool TryPeek(out T item);
    void Clear();
}