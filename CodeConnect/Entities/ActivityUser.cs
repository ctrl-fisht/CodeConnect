namespace CodeConnect.Entities;

public class ActivityUser
{
    public int ActivityUserId { get; set; }
    public int ActivityId { get; set; }
    public Guid UserId { get; set; }

    public required Activity Activity { get; set; }
    public required User User { get; set; }
}
