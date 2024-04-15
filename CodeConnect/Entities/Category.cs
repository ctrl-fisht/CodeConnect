namespace CodeConnect.Entities;

public class Category
{
    public int CategoryId { get; set; }
    public required string Title { get; set; }
    public string ColorHex { get; set; } = "";


    public IList<ActivityCategory> ActivityCategories { get; set; } = new List<ActivityCategory>();
}
