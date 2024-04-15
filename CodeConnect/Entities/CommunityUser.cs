namespace CodeConnect.Entities;

public class CommunityUser
{
    public int CommunityUserId { get; set; }
    public int CommunityId { get; set; }
    public Guid UserId { get; set; }
    public required Community Community { get; set; }
    public required User User { get; set; }
}
