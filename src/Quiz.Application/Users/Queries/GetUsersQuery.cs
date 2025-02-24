using MediatR;
using Quiz.Application.Users.Dtos;
using Quiz.Core.Common;

namespace Quiz.Application.Users.Queries;

public class GetUsersQuery : IRequest<PaginationResult<UsersResponse>>
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}