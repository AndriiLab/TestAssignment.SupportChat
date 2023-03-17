using DotNetCore.CAP;
using SupportChat.Application.Events;
using SupportChat.Application.Interfaces;
using SupportChat.Domain;

namespace SupportChat.Application.Services;

public class AgentCoordinator : IAgentCoordinator
{
    private readonly IJuniorAgentQueue _juniorAgentQueue;
    private readonly IOverflowAgentQueue _overflowAgentQueue;
    private readonly ITeamScheduler _scheduler;
    private readonly IMiddleAgentQueue _middleAgentQueue;
    private readonly ISeniorAgentQueue _seniorAgentQueue;
    private readonly ITeamLeadAgentQueue _teamLeadAgentQueue;
    private readonly ICapPublisher _bus;
    private readonly IDatabase _database;

    public AgentCoordinator(IDatabase database, IJuniorAgentQueue juniorAgentQueue, IMiddleAgentQueue middleAgentQueue,
        ISeniorAgentQueue seniorAgentQueue, ITeamLeadAgentQueue teamLeadAgentQueue, ICapPublisher bus, IOverflowAgentQueue overflowAgentQueue,
        ITeamScheduler scheduler)
    {
        _database = database;
        _juniorAgentQueue = juniorAgentQueue;
        _middleAgentQueue = middleAgentQueue;
        _seniorAgentQueue = seniorAgentQueue;
        _teamLeadAgentQueue = teamLeadAgentQueue;
        _bus = bus;
        _overflowAgentQueue = overflowAgentQueue;
        _scheduler = scheduler;
    }

    public int? GetNextAgentId()
    {
        if (_juniorAgentQueue.TryDequeue(out var agentId))
        {
            return agentId;
        }
        
        if (_overflowAgentQueue.TryDequeue(out agentId))
        {
            return agentId;
        }

        if (_middleAgentQueue.TryDequeue(out agentId))
        {
            return agentId;
        }

        if (_seniorAgentQueue.TryDequeue(out agentId))
        {
            return agentId;
        }
        
        if (_teamLeadAgentQueue.TryDequeue(out agentId))
        {
            return agentId;
        }
        
        _bus.Publish(AgentQueueEmptyEvent.Key, new AgentQueueEmptyEvent());

        return null;
    }

    public int GetCurrentTeamCapacity()
    {
        var agents = _scheduler.GetActiveAgentsOfType(TeamType.Regular, TeamType.NightShift);

        return TeamCapacityCalculator.FindCapacity(agents.Select(a => a.Skill).ToArray());
    }

    public void EnqueueAgent(int agentId)
    {
        if (!_scheduler.CanAgentBeRescheduled(agentId))
        {
            return;
        }
        
        var agent = _database.Agents.FirstOrDefault(a => a.Id == agentId);
        if (agent is null)
        {
            return;
        }

        PushAgentsToQueue(agent);
    }

    public void RequeueAgent(int agentId)
    {
        EnqueueAgent(agentId); // TODO: should append to start of queue method
    }

    public void EnqueueRegularAgents()
    {
        var agents = _scheduler.GetActiveAgentsOfType(TeamType.Regular, TeamType.NightShift);
        PushAgentsToQueue(agents);
    }

    public void EnqueueOverflowAgents()
    {
        var agents = _scheduler.GetActiveAgentsOfType(TeamType.Overflow);
        PushAgentsToQueue(agents);
    }

    public void EmptyOverflowQueue()
    {
        _overflowAgentQueue.Clear();
    }
    
    private void PushAgentsToQueue(params Agent[] agents)
    {
        var agentsDict = new Dictionary<int, List<int>>();
        const int overflowKey = -1;

        foreach (var agent in agents)
        {
            var key = agent.AgentTeam.Type == TeamType.Overflow ? overflowKey : (int)agent.Skill;
            if (!agentsDict.ContainsKey(key))
            {
                agentsDict.Add(key, new List<int>());
            }
            agentsDict[key].Add(agent.Id);
        }

        foreach (var (key, agentList) in agentsDict)
        {
            var skill = key == overflowKey ? AgentSkill.Junior : (AgentSkill)key;
            var capacity = TeamCapacityCalculator.FindCapacity(skill);
            IAgentQueue queue = key switch
            {
                overflowKey => _overflowAgentQueue,
                (int)AgentSkill.Junior => _juniorAgentQueue,
                (int)AgentSkill.Middle => _middleAgentQueue,
                (int)AgentSkill.Senior => _seniorAgentQueue,
                (int)AgentSkill.TeamLead => _teamLeadAgentQueue,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            foreach (var unused in Enumerable.Range(1, capacity))
            {
                foreach (var agent in agentList)
                {
                    queue.Enqueue(agent);
                }
            }
        }
    }
}

public static class TeamCapacityCalculator
{
    private const int BaseCapacity = 10;
    public static int FindCapacity(params AgentSkill[] skills)
    {
        return (int) skills.Sum(s => GetMultiplier(s) * BaseCapacity);
    }

    private static double GetMultiplier(AgentSkill skill)
    {
        return skill switch
        {
            AgentSkill.Junior => 0.4,
            AgentSkill.Middle => 0.6,
            AgentSkill.Senior => 0.8,
            AgentSkill.TeamLead => 0.5,
            _ => throw new ArgumentOutOfRangeException(nameof(skill), skill, null)
        };
    }
}