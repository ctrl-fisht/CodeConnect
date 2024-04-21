namespace CodeConnect.Features.Activities;

public class UpdateActivitySmallPhotoResult
{
    public UpdateActivitySmallPhotoStatus Status { get; set; }
}

public enum UpdateActivitySmallPhotoStatus
{
    Successful,
    UserDoesntExist,
    ActivityDoesntExist,
    UserHasNoAccess,
    FileTooBig,
    IncorrectFormat,
    ErrorWhileUpdating
}
