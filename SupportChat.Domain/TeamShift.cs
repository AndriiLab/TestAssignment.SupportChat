namespace SupportChat.Domain;

public class TeamShift
{
    public int AgentTeamId { get; set; }
    public AgentTeam AgentTeam { get; set; }

    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}