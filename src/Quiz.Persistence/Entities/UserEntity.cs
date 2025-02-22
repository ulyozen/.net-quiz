using Microsoft.AspNetCore.Identity;

namespace Quiz.Persistence.Entities;

public class UserEntity : IdentityUser
{
    public string Name { get; set; }
    
    public bool RememberMe { get; set; }
    
    public bool IsBlocked { get; set; }
    
    public IList<TemplateEntity> Templates { get; set; }
    
    public IList<LikeEntity> Likes { get; set; }
    
    public IList<RefreshTokenEntity> RefreshTokens { get; set; }
    
    public IList<AllowedUsers> AllowedTemplates { get; set; }
}