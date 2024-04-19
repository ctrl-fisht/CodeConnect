namespace CodeConnect.Features.Activities;

public class DeleteActivityResult
{
    public DeleteActivityStatus Status { get; set; }
}

public enum DeleteActivityStatus
{
    Successful,
    ErrorWhileDeleting,
    UserDoesntExist,
    ActivityDoesntExist,
    UserHasNoAccess,
}