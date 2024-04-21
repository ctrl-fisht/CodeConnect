namespace CodeConnect.Entities;

public class ActivityImage
{
    public int ActivityImageId { get; set; }
    public string? SmallPath { get; set; }
    public string? BannerPath { get; set; }

    public int ActivityId { get; set; }

}
