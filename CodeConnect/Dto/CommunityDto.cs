namespace CodeConnect.Dto;

public class CommunityDto
{
    
    public int CommunityId { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = "";
    
    public CommunityImageDto Image { get; set; }
}
