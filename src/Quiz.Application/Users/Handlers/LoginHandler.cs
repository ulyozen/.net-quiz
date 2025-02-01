using MediatR;
using Quiz.Application.Abstractions;
using Quiz.Application.Common;
using Quiz.Application.Users.Commands;
using Quiz.Core.Abstractions;

namespace Quiz.Application.Users.Handlers;

public class LoginHandler(IAuthRepository repo, IJwtGenerator jwt) : IRequestHandler<Login, AuthResponse>
{
    public async Task<AuthResponse> Handle(Login command, CancellationToken cancellationToken)
    {
        var result = await repo.Login(command.Email!, command.Password!);

        var token = jwt.GenerateToken(result.Data!);
        
        return !result.Success 
            ? new AuthResponse { Success = false, Errors = result.Errors } 
            : new AuthResponse { Success = true, Token = token };
    }
}