namespace Quiz.Application.Common;

public class TokenResponse
{
    public int? ExpiresIn { get; set; }
    
    public string? AccessToken { get; set; }
}