namespace CodeConnect.Entities;

public class CommunityImage
{
    public int CommunityImageId { get; set; }
    public string? SmallPath { get; set; }
    public string? MiniPath { get; set; }

    public int CommunityId { get; set; }
}
