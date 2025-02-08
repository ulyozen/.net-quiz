using Quiz.Application.Users.Commands;
using Quiz.Core.Entities;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Common;

public static class UserMapper
{
    public static User MapToUser(this UserEntity userEntity)
    {
        return new User
        {
            Id = userEntity.Id,
            Username = userEntity.Name,
            Email = userEntity.Email,
        };
    }
}