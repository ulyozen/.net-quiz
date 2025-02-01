using Microsoft.AspNetCore.Identity;

namespace Quiz.Persistence.Entities;

public class UserEntity : IdentityUser
{
    public IList<RefreshTokenEntity> RefreshToken { get; set; }
}