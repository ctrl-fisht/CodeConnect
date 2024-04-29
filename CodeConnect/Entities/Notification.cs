namespace CodeConnect.Entities;

public class Notification
{
    public int NotificationId { get; set; }
    public long TgUserId { get; set; }
    public Activity Activity { get; set; }
    public bool SentFirst { get; set; }
    public bool SentSecond { get; set; }
}
