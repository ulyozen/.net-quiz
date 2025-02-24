using MediatR;
using Quiz.Application.Abstractions;
using Quiz.Application.Common;
using Quiz.Application.Mappers;
using Quiz.Application.Users.Commands.AuthActions;
using Quiz.Application.Users.Dtos;
using Quiz.Core.Repositories;

namespace Quiz.Application.Users.Handlers.AuthActions;

public class SignUpHandler(IAuthRepository repo) : IRequestHandler<SignUpCommand, IResponse>
{
    public async Task<IResponse> Handle(SignUpCommand command, CancellationToken cancellationToken)
    {
        var result = await repo.AddUserAsync(command.MapToUser());
        
        return !result.Success
            ? new ErrorResponse(result.Message, result.Errors)
            : new SuccessResponse<string>(result.Data!.Id);
    }
}