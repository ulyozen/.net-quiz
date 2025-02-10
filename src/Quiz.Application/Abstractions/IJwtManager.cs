using Quiz.Application.Common;
using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Application.Abstractions;

public interface IJwtManager
{
    Task<OperationResult<User>> GetRefreshTokenAsync();
    
    Task<AuthResponse> GenerateTokensAsync(User user);
}