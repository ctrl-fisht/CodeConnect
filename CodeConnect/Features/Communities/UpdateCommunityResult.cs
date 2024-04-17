namespace CodeConnect.Features.Communities;

public class UpdateCommunityResult
{
    public UpdateCommunityStatus Status { get; set; }
}

public enum UpdateCommunityStatus
{
    Successful,
    UserDoesntExists,
    UserHasNoAccess,
    CommunityDoesntExists,
    ErrorWhileUpdating
}