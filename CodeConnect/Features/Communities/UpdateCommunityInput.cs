using System.ComponentModel.DataAnnotations;

namespace CodeConnect.Features.Communities;

public class UpdateCommunityInput
{
    [StringLength(50, MinimumLength = 4, ErrorMessage = "Длина названия группы от 4 до 50 символов")]
    public string? Name { get; set; } = null;

    public string? Description { get; set; } = null;
}
