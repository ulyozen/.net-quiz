using Quiz.Application.Users.Commands.AuthActions;
using Quiz.Application.Users.Dtos;
using Quiz.Core.Common;
using Quiz.Core.Entities;
using Quiz.Core.ValueObjects;

namespace Quiz.Application.Mappers;

public static class UserMapper
{
    public static User MapToUser(this SignUpCommand command, string userId)
    {
        return User.Create(userId, command.Username, command.Email, command.Password);
    }
    
    public static UserResponse MapToUserResponse(this User user)
    {
        return new UserResponse
        {
            Id    = user.Id,
            Email = user.Email.Value,
            Role  = user.Role
        };
    }
    
    public static PaginationResult<UsersResponse> MapToPaginationResult(this PaginationResult<User> users)
    {
        var mappedItems = users.Items.Select(MapToUsersResponse).ToList();
        
        return PaginationResult<UsersResponse>.Create(mappedItems, users.TotalCount, users.Page, users.PageSize);
    }
    
    private static UsersResponse MapToUsersResponse(this User user)
    {
        return new UsersResponse
        {
            Id        = user.Id,
            Username  = user.Username,
            Email     = user.Email.Value,
            Role      = user.Role,
            IsBlocked = user.IsBlocked
        };
    }
}