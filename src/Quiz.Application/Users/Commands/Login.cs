using MediatR;
using Quiz.Application.Common;
using Quiz.Core.Common;

namespace Quiz.Application.Users.Commands;

public class Login : IRequest<AuthResponse>
{
    public string? Email { get; set; }
    
    public string? Password { get; set; }
}