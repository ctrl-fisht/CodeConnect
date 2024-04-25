namespace CodeConnect.Features.Activities;

public class UpdateActivityBannerPhotoResult
{
    public UpdateActivityBannerPhotoStatus Status { get; set; }
}

public enum UpdateActivityBannerPhotoStatus
{
    Successful,
    UserDoesntExist,
    ActivityDoesntExist,
    UserHasNoAccess,
    FileTooBig,
    IncorrectFormat,
    ErrorWhileUpdating
}

