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
    
    public OperationResult SetRefreshTokenCookie(string refreshToken, string expiresIn, bool rememberMe)
    {
        http.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = rememberMe 
                ? DateTime.UtcNow.AddDays(int.Parse(expiresIn))
                : DateTime.UtcNow.AddHours(int.Parse(expiresIn))
        });
        
        return OperationResult.SuccessResult();
    }
    
    public OperationResult RemoveRefreshTokenCookie()
    {
        http.HttpContext?.Response.Cookies.Delete("refreshToken");
        
        return OperationResult.SuccessResult();
    }
}