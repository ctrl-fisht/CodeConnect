namespace CodeConnect.Features.Communities;

public class CreateCommunityResult
{
    public CreateCommunityStatus Status { get; set; }
}
public enum CreateCommunityStatus
{
    Successful,
    AlreadyExists,
    ErrorWhileCreating,
    UserDoesntExists
}