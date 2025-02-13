using MediatR;
using Quiz.Core.Common;

namespace Quiz.Application.Users.Commands.AdminActions;

public class BlockCommand : IRequest<OperationResult>
{
    public string UserId { get; set; }
}