using MediatR;
using Quiz.Application.Abstractions;

namespace Quiz.Application.Users.Commands.AuthActions;

public class SignUpCommand : IRequest<IResponse>
{
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
}