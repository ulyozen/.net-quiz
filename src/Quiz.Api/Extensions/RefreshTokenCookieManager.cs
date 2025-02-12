using Quiz.Application.Abstractions;
using Quiz.Core.Common;

namespace Quiz.Api.Extensions;

public class RefreshTokenCookieManager(IHttpContextAccessor http) : IRefreshTokenCookieManager
{
    public OperationResult<string> GetRefreshTokenCookie()
    {
        var token = http.HttpContext?.Request.Cookies["refreshToken"];
        
        return string.IsNullOrWhiteSpace(token) 
            ? OperationResult<string>.Failure(["Refresh token missing"]) 
            : OperationResult<string>.SuccessResult(token);
    }
    
    public void SetRefreshTokenCookie(string refreshToken, string expiresIn, bool rememberMe)
    {
        var expirationTime = rememberMe
            ? DateTime.UtcNow.AddDays(int.Parse(expiresIn))
            : DateTime.UtcNow.AddHours(int.Parse(expiresIn));
        
        var options = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/",
            Expires = expirationTime
        };
        
        http.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken, options);
    }
    
    public void RemoveRefreshTokenCookie()
    {
        http.HttpContext?.Response.Cookies.Delete("refreshToken");
    }
}