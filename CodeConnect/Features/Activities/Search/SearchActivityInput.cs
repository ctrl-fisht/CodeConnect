using System.ComponentModel.DataAnnotations;

namespace CodeConnect.Features.Activities.Search;

public class SearchActivityInput
{
    [StringLength(100, MinimumLength = 5, ErrorMessage = "Длина названия от 5 до 100 символов")]
    public string? Title { get; set; } = null!;

    public DateOnly? DateLocal { get; set; } = null!;

    public int? CityId { get; set; } = null!;
    public bool? HasStream { get; set; } = null!;
    public bool? FreeTicket { get; set; } = null!;

    public List<int>? TagsIds { get; set; } = null!;
    public List<int>? CategoriesIds { get; set; } = null!;
}
