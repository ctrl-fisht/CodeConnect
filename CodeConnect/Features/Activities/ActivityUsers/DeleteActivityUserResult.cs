namespace CodeConnect.Features.Activities.ActivityUsers;

public class DeleteActivityUserResult
{
    public DeleteActivityUserStatus Status { get; set; }
}
public enum DeleteActivityUserStatus
{
    Successful,
    ActivityDoesntExists,
    UserDoesntExists,
    DoesntExists,
    ErrorWhileDeleting
}
