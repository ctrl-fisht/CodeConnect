namespace CodeConnect.Dto;

public class CityDto
{
    public int CityId { get; set; }
    public required string Name { get; set; }
    public int UtcOffsetHours { get; set; }
}
