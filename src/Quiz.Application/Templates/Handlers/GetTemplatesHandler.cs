using MediatR;
using Quiz.Application.Templates.Queries;
using Quiz.Core.Common;
using Quiz.Core.Entities;
using Quiz.Core.Repositories;

namespace Quiz.Application.Templates.Handlers;

public class GetTemplatesHandler : IRequestHandler<GetTemplatesQuery, PaginationResult<Template>>
{
    private readonly ITemplateRepository _templateRepository;

    public GetTemplatesHandler(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }
    
    public async Task<PaginationResult<Template>> Handle(GetTemplatesQuery query, CancellationToken cancellationToken)
    {
        return await _templateRepository.GetTemplatesAsync(query.Page, query.PageSize);
    }
}