using MediatR;
using Quiz.Core.Common;

namespace Quiz.Application.Users.Commands.AdminActions;

public class ChangeRoleCommand : IRequest<OperationResult>
{
    public string UserId { get; set; }
    
    public string Role { get; set; }
}