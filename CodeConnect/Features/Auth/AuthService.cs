using CodeConnect.Data;
using CodeConnect.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodeConnect.Features.Auth;

public class AuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        #pragma warning disable CS8604
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
        #pragma warning restore CS8602

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            expires: DateTime.Now.AddHours(12),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }

    public async Task<LoginResult> LoginUser(string login, string password)
    {

        var user = await _userManager.FindByNameAsync(login);
        if (user != null && await _userManager.CheckPasswordAsync(user, password))
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                #pragma warning disable CS8604
                new Claim(ClaimTypes.Name, user.UserName),
                #pragma warning restore CS8602

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GetToken(authClaims);

            return new LoginResult()
            {
                Status = LoginStatus.Successful,
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expires = token.ValidTo
            };

        }
        return new LoginResult
        {
            Status = LoginStatus.InvalidCredentials,
        };
    }

    public async Task<RegisterResult> RegisterUser(string username, string email, string password)
    {
        var userExists = await _userManager.FindByNameAsync(username);
        if (userExists != null)
            return new RegisterResult
            {
                Status = RegisterStatus.UserAlreadyExists,
            };

        User user = new()
        {
            Email = email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = username
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!await _roleManager.RoleExistsAsync(UserRoles.User))
            await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
        if (await _roleManager.RoleExistsAsync(UserRoles.User))
            await _userManager.AddToRoleAsync(user, UserRoles.User);

        if (!result.Succeeded)
            return new RegisterResult
            {
                Status = RegisterStatus.IdentityRegisterError,
                Errors = result.Errors.Select(error => error.Description).ToList()
            };

        return new RegisterResult()
        {
            Status = RegisterStatus.Successful
        };
    }

    public async Task<IList<string>> GetRolesByUsernameAsync(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        
        if (user is null) return new List<string>();

        var userRoles = await _userManager.GetRolesAsync(user);
        return userRoles;
    }
}