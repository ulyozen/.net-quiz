using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Quiz.Application.Abstractions;

namespace Quiz.Persistence.Common;

public class UserClaimsTransformation(IUserManager userManager) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = (ClaimsIdentity)principal.Identity!;
        var userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId)) return principal;
        
        var roles = await userManager.GetUserRolesAsync(userId);
        foreach (var role in roles)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
        }
        
        return principal;
    }
}