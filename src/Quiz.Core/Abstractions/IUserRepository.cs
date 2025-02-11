using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Core.Abstractions;

public interface IUserRepository
{
    Task<OperationResult<User>> GetUserByIdAsync(string userId);
    
    Task<OperationResult<User>> GetUserByEmailAsync(string email);
    
    Task<IEnumerable<User>> GetUsersAsync();
    
    Task<OperationResult<User>> GetUserByRefreshTokenAsync(string refreshToken);
    
    Task<OperationResult<User>> UserCredentialsAsync(string email, string password, bool rememberMe);
    
    Task<OperationResult<User>> AddUserAsync(User user);
    
    Task<OperationResult> AddRefreshTokenAsync(string userId, string refreshToken, string expiryDate);

    Task<OperationResult> UpdateRefreshTokenAsync(string oldRefreshToken, string newRefreshToken, string expiryDate);
    
    Task<OperationResult> UpdateUserAsync(User user);
    
    Task<OperationResult> DeleteUserAsync(string userId);
}