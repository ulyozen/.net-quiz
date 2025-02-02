namespace Quiz.Persistence.Common;

public class JwtOptions
{
    public string? Secret { get; set; }
    
    public string? Issuer { get; set; }
    
    public string? Audience { get; set; }
    
    public string? AccessTokenExpiryMinutes { get; set; }
    
    public string? RefreshTokenExpiryDays { get; set; }
}