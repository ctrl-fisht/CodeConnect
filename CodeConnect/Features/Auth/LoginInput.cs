using System.ComponentModel.DataAnnotations;

namespace CodeConnect.Features.Auth;

public class LoginInput
{
    [Required(ErrorMessage = "Необходимо ввести имя пользователя")]
    [StringLength(30, MinimumLength = 4, ErrorMessage = "Имя пользователя должно быть от 4 до 30 символов длины")]
    public required string Username { get; set; }


    [Required(ErrorMessage = "Необходимо ввести пароль")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 100 символов длины")]
    public required string Password { get; set; }
}
