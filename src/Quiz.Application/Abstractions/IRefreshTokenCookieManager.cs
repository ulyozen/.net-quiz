using Quiz.Core.Common;

namespace Quiz.Application.Abstractions;

public interface IRefreshTokenCookieManager
{
    OperationResult<string> GetRefreshTokenCookie();
    
    void SetRefreshTokenCookie(string refreshToken, string expiresIn, bool rememberMe);
    
    void RemoveRefreshTokenCookie(int expiresIn = -1);
}