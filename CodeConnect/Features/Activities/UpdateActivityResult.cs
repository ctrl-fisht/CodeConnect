namespace CodeConnect.Features.Activities;

public class UpdateActivityResult
{
    public UpdateActivityStatus Status { get; set; }
}

public enum UpdateActivityStatus
{
    Successful,
    ErrorWhileUpdating,
    UserDoesntExist,
    ActivityDoesntExist,
    CityDoesntExist,
    CommunityDoesntExist,
    UserHasNoAccess,
    BadTags,
    BadCategories
}
