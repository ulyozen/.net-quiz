using MediatR;
using Quiz.Application.Templates.Queries;
using Quiz.Core.Abstractions;
using Quiz.Core.Common;
using Quiz.Core.Entities;
using Quiz.Core.Repositories;

namespace Quiz.Application.Templates.Handlers;

public class SearchTemplatesHandler(ISearchRepository repo) : IRequestHandler<SearchTemplatesQuery, PaginationResult<Template>>
{
    public async Task<PaginationResult<Template>> Handle(SearchTemplatesQuery query, CancellationToken cancellationToken)
    {
        var result = await repo.SearchTemplatesAsync(query.Query!, query.Page, query.PageSize);
        
        return PaginationResult<Template>.Create(result, result.Count, query.Page, query.PageSize);
    }
}