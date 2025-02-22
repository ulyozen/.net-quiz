using MediatR;
using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Application.Templates.Queries;

public class SearchTemplatesQuery :IRequest<PaginationResult<Template>>
{
    public string? Query { get; set; }
    
    public List<string>? Tags { get; set; }
    
    public string? Topic { get; set; }
    
    public int Page { get; set; }
    
    public int PageSize { get; set; }
}