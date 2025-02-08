using Quiz.Application.Users.Commands;
using Quiz.Core.Entities;

namespace Quiz.Application.Common;

public static class UserMapper
{
    public static User MapToUser(this Create command)
    {
        return new User
        {
            Username = command.Username,
            Email = command.Email,
            Password = command.Password,
            Role = GetRoleFromEmail(command.Email)
        };
    }
    
    public static UserInfo MapToUserInfo(this User user)
    {
        return new UserInfo
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role
        };
    }

    private static string GetRoleFromEmail(string email)
    {
        return email.Contains("admin", StringComparison.OrdinalIgnoreCase) ? "admin" : "user";
    }
}