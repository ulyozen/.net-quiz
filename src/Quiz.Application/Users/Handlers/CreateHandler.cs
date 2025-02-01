using MediatR;
using Quiz.Application.Common;
using Quiz.Application.Users.Commands;
using Quiz.Core.Abstractions;
using Quiz.Core.Entities;

namespace Quiz.Application.Users.Handlers;

public class CreateHandler(IAuthRepository repo) : IRequestHandler<Create, AuthResponse>
{
    public async Task<AuthResponse> Handle(Create command, CancellationToken cancellationToken)
    {
        var result = await repo.Create(command.MapToUser());
        
        return !result.Success
            ? new AuthResponse { Success = false, Errors = result.Errors } 
            : new AuthResponse { Success = true, User = result.Data };
    }
}