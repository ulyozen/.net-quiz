using MediatR;
using Quiz.Application.Templates.Dtos;
using Quiz.Core.Common;

namespace Quiz.Application.Templates.Queries;

public class GetTemplatesQuery : IRequest<PaginationResult<TemplateDto>>
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}