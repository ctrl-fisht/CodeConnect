namespace CodeConnect.Features.Communities;

public class DeleteCommunityResult
{
    public DeleteCommunityStatus Status { get; set; }

}
public enum DeleteCommunityStatus
{
    Successful,
    UserDoesntExists,
    UserHasNoAccess,
    CommunityDoesntExists,
    ErrorWhileDeleting
}