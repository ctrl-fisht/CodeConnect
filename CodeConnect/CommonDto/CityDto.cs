namespace CodeConnect.CommonDto;

public class CityDto
{
    public int CityId { get; set; }
    public required string Name { get; set; }
    public int UtcOffsetHours { get; set; }
}
