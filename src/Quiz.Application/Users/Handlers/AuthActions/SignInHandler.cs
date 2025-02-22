using MediatR;
using Quiz.Application.Abstractions;
using Quiz.Application.Common;
using Quiz.Application.Users.Commands.AuthActions;
using Quiz.Application.Users.Dtos;
using Quiz.Core.Repositories;
using Serilog.Context;

namespace Quiz.Application.Users.Handlers.AuthActions;

public class SignInHandler(IAuthRepository repo, IUserCache cache, IJwtManager jwt) : IRequestHandler<SignInCommand, IResponse>
{
    public async Task<IResponse> Handle(SignInCommand command, CancellationToken cancellationToken)
    {
        var result = await repo.LoginAsync(command.Email, command.Password, command.RememberMe);
        
        if (!result.Success) return new ErrorResponse(result.Message, result.Errors);
        
        var user = result.Data!;
        
        LogContext.PushProperty("UserId", user.Id);
        
        await cache.SetUserRolesAsync(user.Id, [user.Role]);
        
        var asd = await jwt.GenerateTokensAsync(result.Data!);
        
        return asd;
    }
}