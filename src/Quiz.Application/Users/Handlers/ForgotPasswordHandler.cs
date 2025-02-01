using MediatR;
using Quiz.Application.Common;
using Quiz.Application.Users.Commands;

namespace Quiz.Application.Users.Handlers;

public class ForgotPasswordHandler : IRequestHandler<ForgotPassword, AuthResponse>
{
    public Task<AuthResponse> Handle(ForgotPassword request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}