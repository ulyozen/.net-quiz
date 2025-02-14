using Quiz.Application.Abstractions;
using Quiz.Core.Common;

namespace Quiz.Api.Services;

public class RefreshTokenCookieManager(IHttpContextAccessor http) : IRefreshTokenCookieManager
{
    private const string RefreshToken = "refreshToken";
    
    public OperationResult<string> GetRefreshTokenCookie()
    {
        var refreshToken = http.HttpContext?.Request.Cookies[RefreshToken];
        
        return string.IsNullOrWhiteSpace(refreshToken) 
            ? OperationResult<string>.Failure(DomainErrors.Auth.RefreshTokenMissing) 
            : OperationResult<string>.SuccessResult(refreshToken);
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
        
        http.HttpContext?.Response.Cookies.Append(RefreshToken, refreshToken, options);
    }
    
    public void RemoveRefreshTokenCookie()
    {
        http.HttpContext?.Response.Cookies.Delete(RefreshToken);
    }
}