namespace CodeConnect.Entities;

public class Tag
{
    public int TagId { get; set; }
    public required string Title { get; set; }
    public string ColorHex { get; set; } = "";


    public IList<ActivityTag> ActivityTags { get; set; } = new List<ActivityTag>();
}