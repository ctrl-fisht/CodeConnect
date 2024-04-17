using CodeConnect.Data;
using CodeConnect.Entities;
using System.Security.Claims;
using System.Security.Principal;

namespace CodeConnect.Helper;

public static class Utils
{
    //public async static Task<User?> GetUserByClaims(IIdentity? identity, IUserRepository userRepository)
    //{
    //    var claimsIdentity = identity as ClaimsIdentity;
    //    var name = claimsIdentity?.FindFirst(ClaimTypes.Name)?.Value;
    //    return await userRepository.GetUserAsync(name);
    //}
    public static bool IsUserAdmin(IIdentity? identity)
    {
        var claimsIdentity = identity as ClaimsIdentity;
        var rolesClaims = claimsIdentity?.FindAll(ClaimTypes.Role);

        if (rolesClaims is null)
            throw new ArgumentNullException();

        foreach (Claim claim in rolesClaims)
        {
            if (claim.Value == UserRoles.Admin)
                return true;
        }
        return false;
    }
}
