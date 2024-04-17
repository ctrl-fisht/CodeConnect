﻿namespace CodeConnect.Entities;

public class Community
{
    public int CommunityId { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = "";

    public User Owner { get; set; } = null!;
    public IList<Activity> Activities { get; set; } = new List<Activity>();
    public IList<CommunityUser> CommunityUsers { get; set; } = new List<CommunityUser>();
}
