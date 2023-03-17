
namespace SupportChat.Domain;

public class AgentTeam
{
    public int Id { get; set; }
    public TeamType Type { get; set; }
    
    public ICollection<Agent> Agents = new List<Agent>();
    public ICollection<TeamShift> Shifts = new List<TeamShift>();
}

public enum TeamType
{
    Regular = 1,
    NightShift = 2,
    Overflow = 3
}