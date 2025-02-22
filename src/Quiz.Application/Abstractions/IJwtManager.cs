using Quiz.Application.Common;
using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Application.Abstractions;

public interface IJwtManager
{
    Task<OperationResult<User>> GetUserRefreshTokenAsync();
    
    Task<IResponse> GenerateTokensAsync(User user);
}