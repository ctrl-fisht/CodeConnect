using CodeConnect.CommonDto;
using CodeConnect.Entities;
using CodeConnect.Features.Activities.ActivityUsers;
using System.ComponentModel.DataAnnotations;

namespace CodeConnect.Features.Activities;

public class CreateActivityInput
{
    [Required(ErrorMessage = "Введите название мероприятия")]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "Длина названия от 5 до 100 символов")]
    public required string Title { get; set; }

    [Required(ErrorMessage = "Укажите дату мероприятия")]
    public required DateOnly DateLocal { get; set; }

    [Required(ErrorMessage = "Укажите время начала мероприятия")]
    public TimeOnly TimeLocal { get; set; }
    
    [Required(ErrorMessage = "Укажите город")]
    public int CityId { get; set; }

    [Required(ErrorMessage = "Укажите группу ")]
    public int CommunityId { get; set; }

    [StringLength(3000, MinimumLength = 5, ErrorMessage = "Длина описания до от 5 до 3000 символов")]
    public string Description { get; set; } = "Описание отсутствует";

    [StringLength(300, MinimumLength = 5, ErrorMessage = "Длина адреса до 300 символов")]
    public string Address { get; set; } = "Неизвестно";
    public int DurationMinutes { get; set; }
    public bool HasStream { get; set; }

    [StringLength(300, MinimumLength = 5, ErrorMessage = "Длина URL до 300 символов")]
    public string? WebsiteURL { get; set; } = null; 

    [StringLength(300, MinimumLength = 5, ErrorMessage = "Длина URL до 300 символов")]
    public string? StreamURL { get; set; }
    public int TicketPrice { get; set; }

    public List<int> TagsIds { get; set; } = new List<int>();
    public List<int> CategoriesIds { get; set; } = new List<int>();

}
