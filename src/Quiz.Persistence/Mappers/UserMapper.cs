using Quiz.Core.Entities;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Mappers;

public static class UserMapper
{
    public static User MapToUser(this UserEntity user)
    {
        return User.Restore(
            user.Id,
            user.UserName,
            user.Email,
            user.IsBlocked,
            user.RememberMe
        );
    }

    public static RefreshTokenEntity CreateRefreshToken(this User user, 
        string tokenId, string refreshToken, int tokenLifetime)
    {
        return new RefreshTokenEntity
        {
            Id = tokenId,
            Token = refreshToken, 
            Expires = user.RememberMe 
                ? DateTime.UtcNow.AddDays(tokenLifetime)
                : DateTime.UtcNow.AddHours(tokenLifetime),
            IsUsed = false,
            IsRevoked = false,
            UserId = user.Id
        };
    }
    
    public static void UpdateRefreshToken(this RefreshTokenEntity entity, User user,
        string newRefreshToken, int tokenLifetime)
    {
        entity.Token = newRefreshToken;
        entity.Expires = user.RememberMe
            ? DateTime.UtcNow.AddDays(tokenLifetime)
            : DateTime.UtcNow.AddHours(tokenLifetime);
    } 
}