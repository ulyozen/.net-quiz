using MediatR;
using Quiz.Application.Common;

namespace Quiz.Application.Users.Commands;

public class RefreshToken : IRequest<AuthResponse> { }