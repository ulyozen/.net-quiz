using Quiz.Core.Entities;

namespace Quiz.Application.Common;

public class AuthResponse
{
    public bool Success { get; set; }
    
    public List<string>? Errors { get; set; }
    
    public TokenResponse? Token { get; set; }
    
    public UserInfo? User { get; set; }
}