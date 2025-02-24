using MediatR;
using Quiz.Application.Common;
using Quiz.Application.Mappers;
using Quiz.Application.Users.Dtos;
using Quiz.Application.Users.Queries;
using Quiz.Core.Common;
using Quiz.Core.Repositories;

namespace Quiz.Application.Users.Handlers.AdminActions;

public class GetUsersHandler : IRequestHandler<GetUsersQuery, PaginationResult<UsersResponse>>
{
    private readonly IAdminRepository _repo;
    
    public GetUsersHandler(IAdminRepository repo) => _repo = repo;
    
    public async Task<PaginationResult<UsersResponse>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var result = await _repo.GetUsersAsync(query.Page, query.PageSize);

        return result.MapToPaginationResult();
    }
}