using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Core.Repositories;

public interface IAuthRepository
{
    Task<OperationResult<User>> GetUserAsync(string refreshToken);
    
    Task<OperationResult<User>> AddUserAsync(User user);
    
    Task<OperationResult<User>> LoginAsync(string email, string password, bool rememberMe);
    
    Task<OperationResult> AddRefreshTokenAsync(User user, string refreshToken, int tokenLifetime);
    
    Task<OperationResult> UpdateRefreshTokenAsync(User user, string oldRefreshToken, string newRefreshToken, int tokenLifetime);
    
    Task<OperationResult> RecoverPasswordAsync(string email, string password);
    
    Task<OperationResult> RevokeRefreshTokenAsync(string refreshToken);
}