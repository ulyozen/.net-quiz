using MediatR;
using Quiz.Application.Abstractions;
using Quiz.Application.Common;
using Quiz.Application.Users.Commands;
using Quiz.Core.Abstractions;

namespace Quiz.Application.Users.Handlers;

public class LoginHandler(IAuthRepository authRepo, IJwtManager jwt) : IRequestHandler<Login, AuthResponse>
{
    public async Task<AuthResponse> Handle(Login command, CancellationToken cancellationToken)
    {
        var result = await authRepo.Login(command.Email!, command.Password!, command.RememberMe);
        
        return !result.Success
            ? new AuthResponse { Success = false, Errors = result.Errors }
            : await jwt.GenerateTokensAsync(result.Data!);
    }
}