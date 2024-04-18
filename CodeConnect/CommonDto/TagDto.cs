namespace CodeConnect.CommonDto;

public class TagDto
{
    public int TagId { get; set; }
    public required string Title { get; set; }
    public string ColorHex { get; set; } = "";
}
