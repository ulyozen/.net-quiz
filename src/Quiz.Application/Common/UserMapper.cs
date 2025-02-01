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
        };
    }
    
    public static User MapToUser(this Login command)
    {
        return new User
        {
            Email = command.Email
        };
    }
}