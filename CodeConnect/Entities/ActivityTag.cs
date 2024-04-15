namespace CodeConnect.Entities;


public class ActivityTag
{
    public int ActivityTagId { get; set; }
    public int ActivityId { get; set; }
    public int TagId { get; set; }

    public required Activity Activity { get; set; }
    public required Tag Tag { get; set; }
}
