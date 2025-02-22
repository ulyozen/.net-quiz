namespace Quiz.Application.Abstractions;

public interface IUserCache
{
    Task<List<string>> GetUserRolesAsync(string userId);

    Task<bool> UserIsBlockedAsync(string userId);
    
    Task SetUserRolesAsync(string userId, List<string> roles);

    Task SetUserBlockStatusAsync(string userId, bool isBlocked);
}