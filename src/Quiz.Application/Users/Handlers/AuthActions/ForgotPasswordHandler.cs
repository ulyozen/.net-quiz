using MediatR;
using Quiz.Application.Abstractions;
using Quiz.Application.Common;
using Quiz.Application.Users.Commands.AuthActions;
using Quiz.Application.Users.Dtos;
using Quiz.Core.Repositories;

namespace Quiz.Application.Users.Handlers.AuthActions;

public class ForgotPasswordHandler(IAuthRepository repo) : IRequestHandler<ForgotPasswordCommand, IResponse>
{
    public async Task<IResponse> Handle(ForgotPasswordCommand command, CancellationToken cancellationToken)
    {
        var result = await repo.RecoverPasswordAsync(command.Email, command.Password);

        return !result.Success
            ? new ErrorResponse(result.Message, result.Errors)
            : new SuccessResponse<bool>(true);
    }
}