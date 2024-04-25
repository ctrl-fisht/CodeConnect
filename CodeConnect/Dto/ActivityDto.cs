using CodeConnect.Entities;

namespace CodeConnect.Dto;

public class ActivityDto
{
    public int ActivityId { get; set; }
    public required string Title { get; set; }
    public DateOnly DateLocal { get; set; }
    public TimeOnly TimeLocal { get; set; }
    public DateOnly DateUtc { get; set; }
    public TimeOnly TimeUtc { get; set; }

    public string Description { get; set; } = "";
    public string Address { get; set; } = "";
    public int DurationMinutes { get; set; }
    public bool HasStream { get; set; }
    public string? StreamURL { get; set; }
    public bool IsActive { get; set; }
    public int TicketPrice { get; set; }
    public string? WebsiteURL { get; set; }

    public CommunityDto Community { get; set; } = null!;

    public ActivityImageDto Image { get; set; } = null!;

    public CityDto City { get; set; } = null!;
    public List<ActivityCategoryDto> ActivityCategories { get; set; } = null!;
    public List<ActivityTagDto> ActivityTags { get; set; } = null!;
}
