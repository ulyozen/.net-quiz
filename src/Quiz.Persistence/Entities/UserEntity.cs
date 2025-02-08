using Microsoft.AspNetCore.Identity;

namespace Quiz.Persistence.Entities;

public class UserEntity : IdentityUser
{
    public string? Name { get; set; }
    
    public IList<RefreshTokenEntity> RefreshToken { get; set; }
}