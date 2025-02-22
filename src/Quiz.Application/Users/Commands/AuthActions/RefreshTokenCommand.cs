using MediatR;
using Quiz.Application.Abstractions;

namespace Quiz.Application.Users.Commands.AuthActions;

public class RefreshTokenCommand : IRequest<IResponse>;