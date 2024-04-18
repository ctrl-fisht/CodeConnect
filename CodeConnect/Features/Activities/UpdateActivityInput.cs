using System.ComponentModel.DataAnnotations;

namespace CodeConnect.Features.Activities;

public class UpdateActivityInput
{
    [StringLength(100, MinimumLength = 5, ErrorMessage = "Длина названия от 5 до 100 символов")]
    public string? Title { get; set; } = null!;

    public  DateOnly? DateLocal { get; set; } = null!;

    public TimeOnly? TimeLocal { get; set; } = null!;

    public int? CityId { get; set; } = null!;

    public int? CommunityId { get; set; } = null!;

    [StringLength(3000, MinimumLength = 5, ErrorMessage = "Длина описания до от 5 до 3000 символов")]
    public string? Description { get; set; } = null!;

    [StringLength(300, MinimumLength = 5, ErrorMessage = "Длина адреса до 300 символов")]
    public string? Address { get; set; } = null!;
    public int? DurationMinutes { get; set; } = null!;
    public bool? HasStream { get; set; } = null!;

    [StringLength(300, MinimumLength = 5, ErrorMessage = "Длина URL до 300 символов")]
    public string? StreamURL { get; set; } = null!;
    public int? TicketPrice { get; set; } = null!;

    public List<int>? TagsIds { get; set; } = null!;
    public List<int>? CategoriesIds { get; set; } = null!;

}
