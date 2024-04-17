using System.ComponentModel.DataAnnotations;

namespace CodeConnect.Features.Communities;

public class CreateCommunityInput
{
    [Required(ErrorMessage = "Введите навзание группы")]
    [StringLength(50, MinimumLength = 4, ErrorMessage = "Длина названия группы от 4 до 50 символов")]
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;
}
