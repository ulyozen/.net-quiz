using Microsoft.AspNetCore.Identity;
using Quiz.Application.Abstractions;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Services;

public class UserManager(IRedisRepository redis, UserManager<UserEntity> userManager) : IUserManager
{
    private const string Key = "roles";
    
    public async Task<List<string>> GetUserRolesAsync(string userId)
    {
        var cacheKey = $"{Key}:{userId}";

        var roles = await redis.GetAsync<List<string>>(cacheKey);
        if (roles != null) return roles;
        
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return [];
        
        roles = (await userManager.GetRolesAsync(user)).ToList();
        
        await redis.SetAsync(cacheKey, roles, TimeSpan.FromMinutes(30));
        
        return roles;
    }

    public async Task RemoveUserRolesAsync(string userId)
    {
        await redis.RemoveAsync($"{Key}:{userId}");
    }
}