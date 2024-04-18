namespace CodeConnect.Features.Activities.ActivityUsers;

public class CreateActivityUserResult
{
    public CreateActivityUserStatus Status { get; set; }
}

public enum CreateActivityUserStatus
{
    Successful,
    ActivityDoesntExists,
    UserDoesntExists,
    AlreadyExists,
    ErrorWhileCreating
}
