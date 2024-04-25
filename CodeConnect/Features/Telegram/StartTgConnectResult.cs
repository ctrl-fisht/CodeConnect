namespace CodeConnect.Features.Telegram;

public class StartTgConnectResult
{
    public StartTgConnectStatus Status { get; set; }
    public string? ConnectUid { get; set; } = null;
}

public enum StartTgConnectStatus
{
    Successful,
    UserDoesntExist,
    AlreadyConnected,
    CantStartSession
}
