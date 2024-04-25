namespace CodeConnect.Features.Communities;

public class UpdateCommunityPhotoResult
{
    public UpdateCommunityPhotoStatus Status { get; set; }
}
public enum UpdateCommunityPhotoStatus
{
    Successful,
    UserDoesntExist,
    CommunityDoesntExist,
    UserHasNoAccess,
    FileTooBig,
    IncorrectFormat,
    ErrorWhileUpdating
}
