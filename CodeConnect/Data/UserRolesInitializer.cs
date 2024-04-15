using CodeConnect.Entities;
using Microsoft.AspNetCore.Identity;

namespace CodeConnect.Data;

public static class UserRoleInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        string[] roleNames = { UserRoles.Admin, UserRoles.User, UserRoles.Owner };

        IdentityResult roleResult;

        foreach (var role in roleNames)
        {
            var roleExists = await roleManager.RoleExistsAsync(role);

            if (!roleExists)
            {
                roleResult = await roleManager.CreateAsync(new IdentityRole(role));
            }
        }


        var ownerEmail = "ctrlshiftweb@gmail.com";
        // init admin
        if (userManager.FindByEmailAsync(ownerEmail).Result == null)
        {
            User user = new()
            {
                Email = ownerEmail,
                UserName = "handakai",
                Bio = "I'm super admin!"
            };

            IdentityResult result = userManager.CreateAsync(user, "Develop159+").Result;

            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, UserRoles.Admin).Wait();
                userManager.AddToRoleAsync(user, UserRoles.Owner).Wait();
                userManager.AddToRoleAsync(user, UserRoles.User).Wait();
            }
        }
    }
}
