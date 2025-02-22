using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Quiz.Application.Abstractions;
using Serilog.Context;

namespace Quiz.Api.Services;

public class UserClaimsTransformation(IUserCache userCache) : IClaimsTransformation
{
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var identity = (ClaimsIdentity)principal.Identity!;
        var userId = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId)) return principal;
        
        LogContext.PushProperty("UserId", userId);
        
        var roles = await userCache.GetUserRolesAsync(userId);
        foreach (var role in roles)
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
        }
        
        return principal;
    }
}