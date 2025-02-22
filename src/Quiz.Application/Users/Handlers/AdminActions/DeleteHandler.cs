using MediatR;
using Quiz.Application.Users.Commands.AdminActions;
using Quiz.Core.Abstractions;
using Quiz.Core.Common;
using Quiz.Core.Repositories;

namespace Quiz.Application.Users.Handlers.AdminActions;

public class DeleteHandler(IAdminRepository repo) : IRequestHandler<DeleteCommand, OperationResult>
{
    public async Task<OperationResult> Handle(DeleteCommand command, CancellationToken cancellationToken)
    {
        return await repo.DeleteUserAsync(command.UserId);
    }
}