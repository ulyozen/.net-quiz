using MediatR;
using Quiz.Application.Mappers;
using Quiz.Application.Templates.Dtos;
using Quiz.Application.Templates.Queries;
using Quiz.Core.Abstractions;
using Quiz.Core.Common;
using Quiz.Core.Entities;
using Quiz.Core.Repositories;

namespace Quiz.Application.Templates.Handlers;

public class SearchTemplatesHandler : IRequestHandler<SearchTemplatesQuery, PaginationResult<TemplateDto>>
{
    private readonly ISearchRepository _repository;
    
    public SearchTemplatesHandler(ISearchRepository repository) => _repository = repository;
    
    public async Task<PaginationResult<TemplateDto>> Handle(SearchTemplatesQuery query, CancellationToken cancellationToken)
    {
        var result = await _repository.SearchTemplatesAsync(query.Query, query.Page, query.PageSize);
        
        return PaginationResult<TemplateDto>.Create(result.MapToDto(), result.Count, query.Page, query.PageSize);
    }
}