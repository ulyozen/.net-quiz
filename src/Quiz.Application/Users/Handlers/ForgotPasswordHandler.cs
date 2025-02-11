using MediatR;
using Quiz.Application.Common;
using Quiz.Application.Users.Commands;
using Quiz.Core.Abstractions;

namespace Quiz.Application.Users.Handlers;

public class ForgotPasswordHandler(IAuthRepository repo) : IRequestHandler<ForgotPassword, AuthResponse>
{
    public async Task<AuthResponse> Handle(ForgotPassword command, CancellationToken cancellationToken)
    {
        var result = await repo.ForgotPassword(command.Email, command.Password);

        return !result.Success
            ? new AuthResponse { Success = false, Errors = result.Errors }
            : new AuthResponse { Success = true };
    }
}