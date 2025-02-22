using MediatR;
using Quiz.Application.Abstractions;
using Quiz.Application.Users.Commands.AdminActions;
using Quiz.Core.Abstractions;
using Quiz.Core.Common;
using Quiz.Core.Repositories;

namespace Quiz.Application.Users.Handlers.AdminActions;

public class BlockHandler(IAdminRepository repo, IUserCache cache) : IRequestHandler<BlockCommand, OperationResult>
{
    public async Task<OperationResult> Handle(BlockCommand command, CancellationToken cancellationToken)
    {
        var result = await repo.BlockUserAsync(command.UserId);

        if (result.Success) await cache.SetUserBlockStatusAsync(command.UserId, true);

        return result;
    }
}