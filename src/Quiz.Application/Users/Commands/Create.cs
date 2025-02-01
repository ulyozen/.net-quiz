using MediatR;
using Quiz.Application.Common;

namespace Quiz.Application.Users.Commands;

public class Create : IRequest<AuthResponse>
{
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
}