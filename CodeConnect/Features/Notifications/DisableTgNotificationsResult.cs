namespace CodeConnect.Features.Notifications;

public class DisableTgNotificationsResult
{
    public DisableTgNotificationsStatus Status { get; set; }
}
public enum DisableTgNotificationsStatus
{
    Successful,
    UserDoesntExist,
    AlreadyDisabled,
    ErrorWhileDisabled
}
