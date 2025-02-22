using MediatR;
using Quiz.Application.Abstractions;
using Quiz.Application.Users.Commands.AdminActions;
using Quiz.Core.Abstractions;
using Quiz.Core.Common;
using Quiz.Core.Repositories;

namespace Quiz.Application.Users.Handlers.AdminActions;

public class UnblockHandler(IAdminRepository repo, IUserCache cache) : IRequestHandler<UnblockCommand, OperationResult>
{
    public async Task<OperationResult> Handle(UnblockCommand command, CancellationToken cancellationToken)
    {
        var result = await repo.UnblockUserAsync(command.UserId);
        
        if (result.Success) await cache.SetUserBlockStatusAsync(command.UserId, false);

        return result;
    }
}