using System.ComponentModel.DataAnnotations;

namespace CodeConnect.Features.Auth;

public class RegisterInput
{
    [Required(ErrorMessage = "Введите имя пользователя")]
    [StringLength(30, MinimumLength = 4, ErrorMessage = "Имя пользователя должно быть от 4 до 30 символов длины")]
    public required string Username { get; set; }


    [Required(ErrorMessage = "Введите адрес электронной почты")]
    [EmailAddress(ErrorMessage = "Некорректный почтовый адрес")]
    [StringLength(254, ErrorMessage = "Максимальная длина адреса электронной почты 254 символов")]
    public required string Email { get; set; }


    [Required(ErrorMessage = "Введите пароль")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 100 символов длины")]
    public required string Password { get; set; }
}
