namespace CodeConnect.Entities;

public class CommunityUser
{
    public int CommunityUserId { get; set; }
    public int CommunityId { get; set; }
    public required string UserId { get; set; }
    public Community Community { get; set; } = null!;
    public User User { get; set; } = null!;
}
