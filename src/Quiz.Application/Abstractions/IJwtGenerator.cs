using Quiz.Application.Common;
using Quiz.Core.Entities;

namespace Quiz.Application.Abstractions;

public interface IJwtGenerator
{
    TokenResponse GenerateToken(User user);
    
    string UpdateRefreshToken(string refreshToken);
}