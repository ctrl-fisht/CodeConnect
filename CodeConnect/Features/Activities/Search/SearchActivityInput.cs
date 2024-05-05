using System.ComponentModel.DataAnnotations;

namespace CodeConnect.Features.Activities.Search;

public class SearchActivityInput
{
    public string? Title { get; set; } = null!;

    [MaxLength(2)]
    [MinLength(1)]
    public DateOnly[]? DateRange { get; set; } = null!;

    public int? CityId { get; set; } = null!;
    public bool? HasStream { get; set; } = null!;
    public bool? FreeTicket { get; set; } = null!;

    public bool? Past { get; set; } = null!;

    public List<int>? TagsIds { get; set; } = null!;
    public List<int>? CategoriesIds { get; set; } = null!;
}
