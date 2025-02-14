namespace Quiz.Application.Abstractions;

public interface IUserManager
{
    Task<List<string>> GetUserRolesAsync(string userId);

    Task RemoveUserRolesAsync(string userId);
}