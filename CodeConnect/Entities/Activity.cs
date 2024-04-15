namespace CodeConnect.Entities;

public class Activity
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


    public required User Owner { get; set; }
    public required Community Community { get; set; }
    public IList<ActivityCategory> ActivityCategories { get; set; } = new List<ActivityCategory>();
    public IList<ActivityTag> ActivityTags { get; set; } = new List<ActivityTag>();
    public IList<ActivityUser> Members { get; set; } = new List<ActivityUser>();

}
