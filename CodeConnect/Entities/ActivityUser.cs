namespace CodeConnect.Entities;

public class ActivityUser
{
    public int ActivityUserId { get; set; }
    public int ActivityId { get; set; }
    public required string UserId { get; set; }

    public Activity Activity { get; set; } = null!;
    public User User { get; set; } = null!;
}
