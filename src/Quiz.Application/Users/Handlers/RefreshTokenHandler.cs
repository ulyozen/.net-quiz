using MediatR;
using Quiz.Application.Abstractions;
using Quiz.Application.Common;
using Quiz.Application.Users.Commands;

namespace Quiz.Application.Users.Handlers;

public class RefreshTokenHandler(IJwtManager jwt) : IRequestHandler<RefreshToken, AuthResponse>
{
    public async Task<AuthResponse> Handle(RefreshToken request, CancellationToken cancellationToken)
    {
        var result = await jwt.GetUserRefreshTokenAsync();
        
        return !result.Success
            ? new AuthResponse { Success = false, Errors = result.Errors }
            : await jwt.GenerateTokensAsync(result.Data!);
    }
}