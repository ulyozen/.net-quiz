using Quiz.Core.Common;

namespace Quiz.Application.Abstractions;

public interface IRefreshTokenCookieManager
{
    OperationResult<string> GetRefreshTokenCookie();
    
    void SetRefreshTokenCookie(string refreshToken, int expiresIn, bool rememberMe);
    
    void RemoveRefreshTokenCookie();
}