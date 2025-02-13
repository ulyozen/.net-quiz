using MediatR;
using Quiz.Application.Abstractions;
using Quiz.Application.Users.Commands;
using Quiz.Core.Abstractions;

namespace Quiz.Application.Users.Handlers;

public class LogoutHandler(IAuthRepository repo) : IRequestHandler<Logout>
{
    public Task Handle(Logout command, CancellationToken cancellationToken)
    {
        return repo.RevokeAccessAsync();
    }
}