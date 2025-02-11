using Quiz.Core.Common;

namespace Quiz.Application.Abstractions;

public interface IRefreshTokenCookieManager
{
    OperationResult<string> GetRefreshTokenCookie();
    
    OperationResult SetRefreshTokenCookie(string refreshToken, string expiresIn, bool rememberMe);
    
    OperationResult RemoveRefreshTokenCookie();
}