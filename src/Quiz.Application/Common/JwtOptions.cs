using System.ComponentModel.DataAnnotations;

namespace Quiz.Application.Common;

public class JwtOptions
{
    [Required]
    public string JWT_SECRET { get; set; }
    
    [Required]
    public string JWT_ISSUER { get; set; }
    
    [Required]
    public string JWT_AUDIENCE { get; set; }
    
    [Range(1, 30)]
    public int ACCESS_TOKEN_EXPIRY_MINUTES { get; set; }
    
    [Range(1, 30)]
    public int REFRESH_TOKEN_EXPIRY_DAYS { get; set; }
}