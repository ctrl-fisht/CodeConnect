namespace CodeConnect.Entities;

public class City
{
    public int CityId { get; set; }
    public required string Name { get; set; }
    public int UtcOffsetHours{ get; set; }


    public IList<Activity> Activities { get; set; } = new List<Activity>();
    public IList<User> Users { get; set; } = new List<User>();
}
