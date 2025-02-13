using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Core.Abstractions;

public interface IAuthRepository
{
    Task<OperationResult<User>> GetUserAsync(string refreshToken);
    
    Task<OperationResult<User>> AddUserAsync(User user);
    
    Task<OperationResult<User>> LoginAsync(string email, string password, bool rememberMe);
    
    Task<OperationResult> AddRefreshTokenAsync(User user, string refreshToken, string expiryDate);
    
    Task<OperationResult> UpdateRefreshTokenAsync(User user, string oldRefreshToken, string newRefreshToken, string expiryDate);
    
    Task<OperationResult> RecoverPasswordAsync(string email, string password);
    
    Task<OperationResult> RevokeAccessAsync();
}