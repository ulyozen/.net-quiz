using MediatR;
using Quiz.Application.Abstractions;
using Quiz.Application.Users.Commands.AdminActions;
using Quiz.Core.Abstractions;
using Quiz.Core.Common;
using Quiz.Core.Repositories;

namespace Quiz.Application.Users.Handlers.AdminActions;

public class ChangeRoleHandler(IAdminRepository repo, IUserCache cache) : IRequestHandler<ChangeRoleCommand, OperationResult>
{
    public async Task<OperationResult> Handle(ChangeRoleCommand command, CancellationToken cancellationToken)
    {
        var result = await repo.ChangeRoleAsync(command.UserId, command.Role);
        
        if (result.Success) await cache.SetUserRolesAsync(command.UserId, [command.Role]);
        
        return result;
    }
}