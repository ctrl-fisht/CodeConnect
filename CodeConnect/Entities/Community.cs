﻿namespace CodeConnect.Entities;

public class Community
{
    public int CommunityId { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = "";

    public string? Email { get; set; } = null;
    public string? Phone { get; set; } = null;
    public string? TelegramTag { get; set; } = null;
    
    public bool Deleted { get; set; } = false;

    public CommunityImage Image { get; set; } = null!;
    public User Owner { get; set; } = null!;
    public IList<Activity> Activities { get; set; } = new List<Activity>();
    public IList<CommunityUser> CommunityUsers { get; set; } = new List<CommunityUser>();
}