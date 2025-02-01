namespace Quiz.Persistence.Common;

public class JwtOptions
{
    public string? Secret { get; set; }
    
    public string? Issuer { get; set; }
    
    public string? Audience { get; set; }
    
    public int AccessTokenExpiryMinutes { get; set; } = 30;
    
    public int RefreshTokenExpiryDays { get; set; } = 7;
}