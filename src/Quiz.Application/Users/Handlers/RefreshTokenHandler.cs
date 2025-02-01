using MediatR;
using Quiz.Application.Common;
using Quiz.Application.Users.Commands;

namespace Quiz.Application.Users.Handlers;

public class RefreshTokenHandler : IRequestHandler<RefreshToken, AuthResponse>
{
    public Task<AuthResponse> Handle(RefreshToken request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}