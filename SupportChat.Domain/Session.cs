namespace SupportChat.Domain;

public class Session
{
    public Session()
    {
        IsActive = true;
        PingDate = DateTime.Now;
    }

    public int Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime PingDate { get; set; }
    public int? AgentId { get; set; }
    public Agent? Agent { get; set; }
}