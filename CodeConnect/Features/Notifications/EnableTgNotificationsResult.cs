namespace CodeConnect.Features.Notifications;

public class EnableTgNotificationsResult
{
    public EnableTgNotificationsStatus Status { get; set; }
}
public enum EnableTgNotificationsStatus
{
    Successful,
    TgIdNull,
    UserDoesntExist,
    AlreadyEnabled,
    ErrorWhileEnabling
}
