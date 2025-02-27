using MediatR;
using Quiz.Application.Abstractions;
using Quiz.Application.Common;
using Quiz.Application.Mappers;
using Quiz.Application.Users.Commands.AuthActions;
using Quiz.Application.Users.Dtos;
using Quiz.Core.Abstractions;
using Quiz.Core.Repositories;

namespace Quiz.Application.Users.Handlers.AuthActions;

public class SignUpHandler : IRequestHandler<SignUpCommand, IResponse>
{
    private readonly IAuthRepository _repo;
    private readonly IGuidFactory _guidFactory;
    private readonly IDomainEventDispatcher _dispatcher;

    public SignUpHandler(
        IAuthRepository repo, 
        IGuidFactory guidFactory,
        IDomainEventDispatcher dispatcher)
    {
        _repo = repo;
        _guidFactory = guidFactory;
        _dispatcher = dispatcher;
    }
    
    public async Task<IResponse> Handle(SignUpCommand command, CancellationToken cancellationToken)
    {
        var result = await _repo.AddUserAsync(command.MapToUser(_guidFactory.Create()));
        
        if (!result.Success)
            return new ErrorResponse(result.Message, result.Errors);

        var user = result.Data!;

        await _dispatcher.Dispatch(user.GetDomainEvents());
        
        user.ClearDomainEvents();
        
        return new SuccessResponse<string>(user.Id);
    }
}