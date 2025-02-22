using MediatR;
using Quiz.Application.Abstractions;

namespace Quiz.Application.Users.Commands.AuthActions;

public class ForgotPasswordCommand : IRequest<IResponse>
{
    public string Email { get; set; }
    
    public string Password { get; set; }
}