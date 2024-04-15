using Microsoft.AspNetCore.Identity;

namespace CodeConnect.Entities;

public class User : IdentityUser
{
    public string Bio { get; set; } = "";

    public City? City { get; set; } = null!;
    public IList<ActivityUser> ActivityUsers { get; set; } = new List<ActivityUser>();
    public IList<CommunityUser> CommunityUsers { get; set; } = new List<CommunityUser>();
    public IList<Activity> ActivitiesOwned { get; set; } = new List<Activity>();
    public IList<Community> CommunitiesOwned { get; set; } = new List<Community>();

}
