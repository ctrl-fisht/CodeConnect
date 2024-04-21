namespace CodeConnect.Dto;

public class CategoryDto
{
    public int CategoryId { get; set; }
    public required string Title { get; set; }
    public string ColorHex { get; set; } = "";
}
