namespace SupportChat.Domain;

public class Agent
{
    public int Id { get; set; }
    public AgentSkill Skill { get; set; }
    public int AgentTeamId { get; set; }
    public AgentTeam AgentTeam { get; set; }
}

public enum AgentSkill
{
    Junior = 1,
    Middle = 2,
    Senior = 3,
    TeamLead = 4
}