using CodeConnect.Features.Communities;

namespace CodeConnect.Features.Communities.CommunityUsers;

public class CreateCommunityUserResult
{
    public CreateCommunityUserStatus Status { get; set; }

}

public enum CreateCommunityUserStatus
{
    Successful,
    CommunityDoesntExists,
    UserDoesntExists,
    AlreadyExists,
    ErrorWhileCreating
}