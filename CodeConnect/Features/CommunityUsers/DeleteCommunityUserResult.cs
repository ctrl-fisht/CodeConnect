namespace CodeConnect.Features.CommunityUsers;

public class DeleteCommunityUserResult
{
    public DeleteCommunityUserStatus Status { get; set; }
}

public enum DeleteCommunityUserStatus
{
    Successful,
    CommunityDoesntExists,
    UserDoesntExists,
    DoesntExists,
    ErrorWhileDeleting
}