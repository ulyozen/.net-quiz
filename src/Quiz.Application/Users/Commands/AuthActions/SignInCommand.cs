using MediatR;
using Quiz.Application.Abstractions;

namespace Quiz.Application.Users.Commands.AuthActions;

public class SignInCommand : IRequest<IResponse>
{
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public bool RememberMe { get; set; }
}