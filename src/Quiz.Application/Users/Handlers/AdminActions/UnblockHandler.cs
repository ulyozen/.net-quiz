using MediatR;
using Quiz.Application.Users.Commands.AdminActions;
using Quiz.Core.Abstractions;
using Quiz.Core.Common;

namespace Quiz.Application.Users.Handlers.AdminActions;

public class UnblockHandler(IAdminRepository repo) : IRequestHandler<UnblockCommand, OperationResult>
{
    public async Task<OperationResult> Handle(UnblockCommand command, CancellationToken cancellationToken)
    {
        return await repo.UnblockUserAsync(command.UserId);
    }
}