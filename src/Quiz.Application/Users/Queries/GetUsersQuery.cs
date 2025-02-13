using MediatR;
using Quiz.Core.Entities;

namespace Quiz.Application.Users.Queries;

public class GetUsersQuery : IRequest<IEnumerable<User>>;