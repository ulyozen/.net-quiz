namespace Quiz.Application.Users.Dtos;

public class TokenResponse
{
    public int? ExpiresIn { get; set; }
    
    public string? AccessToken { get; set; }
}