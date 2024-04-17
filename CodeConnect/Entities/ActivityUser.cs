namespace CodeConnect.Entities;

public class ActivityUser
{
    public int ActivityUserId { get; set; }
    public int ActivityId { get; set; }
    public required string UserId { get; set; }

    public required Activity Activity { get; set; }
    public required User User { get; set; }
}
