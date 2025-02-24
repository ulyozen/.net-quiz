using MediatR;
using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Application.Templates.Queries;

public class GetTemplatesQuery : IRequest<PaginationResult<Template>>
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}