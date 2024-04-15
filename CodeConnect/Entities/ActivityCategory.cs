namespace CodeConnect.Entities;

public class ActivityCategory
{
    public int ActivityCategoryId { get; set; }
    public int ActivityId { get; set; }
    public int CategoryId { get; set; }
    public required Activity Activity { get; set; }
    public required Category Category { get; set; }
}
