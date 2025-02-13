using MediatR;
using Quiz.Application.Users.Commands.AdminActions;
using Quiz.Core.Abstractions;
using Quiz.Core.Common;

namespace Quiz.Application.Users.Handlers.AdminActions;

public class ChangeRoleHandler(IAdminRepository repo) : IRequestHandler<ChangeRoleCommand, OperationResult>
{
    public async Task<OperationResult> Handle(ChangeRoleCommand command, CancellationToken cancellationToken)
    {
        return await repo.ChangeRoleAsync(command.UserId, command.Role);
    }
}