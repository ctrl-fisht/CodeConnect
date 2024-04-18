namespace CodeConnect.Features.Communities.CommunityUsers;

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