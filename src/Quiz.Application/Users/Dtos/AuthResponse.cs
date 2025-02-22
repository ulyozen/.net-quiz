using Quiz.Application.Abstractions;
using Quiz.Application.Common;

namespace Quiz.Application.Users.Dtos;

public class AuthResponse : BaseResponse
{
    public TokenResponse Token { get; private set; }
    
    public UserResponse User { get; private set; }
    
    public AuthResponse(TokenResponse token, UserResponse user)
    {
        Success = true;
        Token = token;
        User = user;
    }
}
