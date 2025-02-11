using Quiz.Application.Abstractions;
using Quiz.Core.Abstractions;
using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Persistence.Repositories;

public class AuthRepository(IUserRepository repo, IRefreshTokenCookieManager cookie) : IAuthRepository
{
    public async Task<OperationResult<User>> Create(User user)
    {
        var result = await repo.AddUserAsync(user);
        
        return !result.Success 
            ? OperationResult<User>.Failure(result.Errors!) 
            : OperationResult<User>.SuccessResult(result.Data!);
    }
    
    public async Task<OperationResult<User>> Login(string email, string password, bool rememberMe)
    {
        var result = await repo.UserCredentialsAsync(email, password, rememberMe);
        
        return !result.Success
            ? OperationResult<User>.Failure(result.Errors!) 
            : OperationResult<User>.SuccessResult(result.Data!);
    }
    
    public async Task<OperationResult> ForgotPassword(string email, string password)
    {
        var result = await repo.UpdateUserPasswordAsync(email, password);
        
        return !result.Success
            ? OperationResult.Failure(result.Errors!) 
            : OperationResult.SuccessResult(); 
    }
    
    public async Task<OperationResult> Logout()
    {
        var refreshToken = cookie.GetRefreshTokenCookie();
        if (!refreshToken.Success)
            return OperationResult.Failure(refreshToken.Errors!);

        cookie.RemoveRefreshTokenCookie();
        
        return await repo.DeleteRefreshTokenAsync(refreshToken.Data!);
    }
}