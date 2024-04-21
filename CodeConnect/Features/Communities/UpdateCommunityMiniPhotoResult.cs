namespace CodeConnect.Features.Communities;

public class UpdateCommunityMiniPhotoResult
{
    public UpdateCommunityMiniPhotoStatus Status { get; set; }
}
public enum UpdateCommunityMiniPhotoStatus
{
    Successful,
    UserDoesntExist,
    CommunityDoesntExist,
    UserHasNoAccess,
    FileTooBig,
    IncorrectFormat,
    ErrorWhileUpdating
}
