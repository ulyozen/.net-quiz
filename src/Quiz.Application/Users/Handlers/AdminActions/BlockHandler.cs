using MediatR;
using Quiz.Application.Users.Commands.AdminActions;
using Quiz.Core.Abstractions;
using Quiz.Core.Common;

namespace Quiz.Application.Users.Handlers.AdminActions;

public class BlockHandler(IAdminRepository repo) : IRequestHandler<BlockCommand, OperationResult>
{
    public async Task<OperationResult> Handle(BlockCommand command, CancellationToken cancellationToken)
    {
        return await repo.BlockUserAsync(command.UserId);
    }
}