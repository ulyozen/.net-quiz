using MediatR;
using Quiz.Application.Abstractions;
using Quiz.Application.Common;
using Quiz.Application.Users.Commands.AuthActions;
using Quiz.Application.Users.Dtos;

namespace Quiz.Application.Users.Handlers.AuthActions;

public class RefreshTokenHandler(IJwtManager jwt) : IRequestHandler<RefreshTokenCommand, IResponse>
{
    public async Task<IResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var result = await jwt.GetUserRefreshTokenAsync();
        
        return !result.Success
            ? new ErrorResponse(result.Message, result.Errors)
            : await jwt.GenerateTokensAsync(result.Data!);
    }
}