using Microsoft.AspNetCore.Identity;
using Quiz.Application.Abstractions;
using Quiz.Persistence.Entities;

namespace Quiz.Redis.Services;

public class UserCache(ICacheManager cache, UserManager<UserEntity> userManager) : IUserCache
{
    private const int CacheDurationInMinutes = 1440;
    
    public async Task<List<string>> GetUserRolesAsync(string userId)
    {
        var cachedRoles = await cache.GetAsync<List<string>>(RoleKey(userId));
        if (cachedRoles.Success) return cachedRoles.Data!;
        
        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return [];
        
        var roles = (await userManager.GetRolesAsync(user)).ToList();
        
        await SetUserRolesAsync(RoleKey(userId), roles);
        
        return roles;
    }
    
    public async Task<bool> UserIsBlockedAsync(string userId)
    {
        var isUserBlockedCache = await cache.GetAsync<bool>(UserBanKey(userId));
        if (isUserBlockedCache.Success) return isUserBlockedCache.Data;
        
        var user = await userManager.FindByIdAsync(userId);
        if (user is null) return false;
        
        await cache.SetAsync(UserBanKey(userId), user.IsBlocked, CacheDurationInMinutes);
        
        return user.IsBlocked;
    }
    
    public async Task SetUserRolesAsync(string userId, List<string> roles)
    {
        await cache.SetAsync(RoleKey(userId), roles);
    }
    
    public async Task SetUserBlockStatusAsync(string userId, bool isBlocked)
    {
        await cache.SetAsync(UserBanKey(userId), isBlocked, CacheDurationInMinutes);
    }
    
    private static string RoleKey(string userId) => $"user:roles:{userId}";
    
    private static string UserBanKey(string userId) => $"user:ban:{userId}";
}