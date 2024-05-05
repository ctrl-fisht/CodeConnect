namespace CodeConnect.Dto;

public class CommunityDto
{
    
    public int CommunityId { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = "";

    public string? Email { get; set; } = null;
    public string? Phone { get; set; } = null;
    public string? TelegramTag { get; set; } = null;

    public CommunityImageDto Image { get; set; }
}
