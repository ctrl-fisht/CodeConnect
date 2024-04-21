namespace CodeConnect.Features.Communities;

public class UpdateCommunitySmallPhotoResult
{
    public UpdateCommunitySmallPhotoStatus Status { get; set; }
}
public enum UpdateCommunitySmallPhotoStatus
{
    Successful,
    UserDoesntExist,
    CommunityDoesntExist,
    UserHasNoAccess,
    FileTooBig,
    IncorrectFormat,
    ErrorWhileUpdating
}
