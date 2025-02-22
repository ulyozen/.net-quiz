using MediatR;
using Quiz.Application.Abstractions;
using Quiz.Application.Users.Commands.AuthActions;
using Quiz.Core.Repositories;

namespace Quiz.Application.Users.Handlers.AuthActions;

public class LogoutHandler(IAuthRepository repo, IRefreshTokenCookieManager cookie) : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        var refreshToken = cookie.GetRefreshTokenCookie();
        
        if (!refreshToken.Success) return;
        
        cookie.RemoveRefreshTokenCookie();
        
        await repo.RevokeRefreshTokenAsync(refreshToken.Data!);
    }
}