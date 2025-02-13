using MediatR;
using Quiz.Application.Users.Queries;
using Quiz.Core.Abstractions;
using Quiz.Core.Entities;

namespace Quiz.Application.Users.Handlers.AdminActions;

public class GetUsersHandler(IAdminRepository repo) : IRequestHandler<GetUsersQuery, IEnumerable<User>>
{
    public async Task<IEnumerable<User>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        return await repo.GetUsersAsync();
    }
}