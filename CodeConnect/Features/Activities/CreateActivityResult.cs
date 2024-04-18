namespace CodeConnect.Features.Activities;

public class CreateActivityResult
{
    public CreateActivityStatus Status { get; set; }
}

public enum CreateActivityStatus
{
    Successful,
    ErrorWhileCreating,
    UserDoesntExist,
    CityDoesntExist,
    CommunityDoesntExist,
    UserHasNoAccess,
    BadTags,
    BadCategories
}
