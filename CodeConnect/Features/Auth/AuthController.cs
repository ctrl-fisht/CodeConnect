using CodeConnect.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CodeConnect.Features.Auth;

[Route("api/auth/")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginInput input)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginUser(input.Username, input.Password);

        switch (result.Status)
        {
            case LoginStatus.InvalidCredentials:
                {
                    return Unauthorized(new { success = false, message = "Неверный логин или пароль" });
                }
            case LoginStatus.Successful:
                {
                    return StatusCode(200,
                    new
                    {
                        success = true,
                        accessToken = result.AccessToken,
                        expires = result.Expires
                    });
                }
            default: throw new ArgumentOutOfRangeException();
        }
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterInput input)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RegisterUser(input.Username, input.Email, input.Password);

        switch (result.Status)
        {
            case RegisterStatus.UserAlreadyExists:
                return StatusCode(409, new { success = false, message = "Пользователь с таким именем уже существует" });

            case RegisterStatus.Successful:
                return StatusCode(201, new { success = true, message = "Пользователь успешно создан" });

            case RegisterStatus.IdentityRegisterError:
                return BadRequest(new { success = false, errors = result.Errors });

            // недосягаемый код т.к. бизнес логика возвращает всегда RegisterStatus в пределах enum
            default: throw new ArgumentOutOfRangeException();
        }
    }


    // Dev Route
    [Authorize]
    [HttpGet]
    [Route("roles")]
    public async Task<IActionResult> GetRole()
    {
        // из за [Authorize] аттрибута claims точно будут.
        #pragma warning disable CS8604
        #pragma warning disable CS8602
        var claimsIdentity = User.Identity as ClaimsIdentity;
        var name = claimsIdentity.FindFirst(ClaimTypes.Name).Value;
        var roles = await _authService.GetRolesByUsernameAsync(name);

        return Ok(roles);
        #pragma warning restore CS8602
        #pragma warning restore CS8604
    }

}
