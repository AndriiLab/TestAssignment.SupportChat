using SupportChat.Core.Queue;

namespace SupportChat.Application.Interfaces;

public interface IAgentQueue : IQueue<int>
{
}

public interface IJuniorAgentQueue : IAgentQueue
{
}

public interface IOverflowAgentQueue : IAgentQueue
{
}


public interface IMiddleAgentQueue : IAgentQueue
{
}

public interface ISeniorAgentQueue : IAgentQueue
{
}

public interface ITeamLeadAgentQueue : IAgentQueue
{
}