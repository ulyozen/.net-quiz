using MediatR;
using Quiz.Application.Mappers;
using Quiz.Application.Templates.Dtos;
using Quiz.Application.Templates.Queries;
using Quiz.Core.Common;
using Quiz.Core.Entities;
using Quiz.Core.Repositories;

namespace Quiz.Application.Templates.Handlers;

public class GetTemplatesHandler : IRequestHandler<GetTemplatesQuery, PaginationResult<TemplateDto>>
{
    private readonly ITemplateRepository _templateRepository;
    
    public GetTemplatesHandler(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }
    
    public async Task<PaginationResult<TemplateDto>> Handle(GetTemplatesQuery query, CancellationToken cancellationToken)
    {
        var result = await _templateRepository.GetTemplatesAsync(query.Page, query.PageSize);

        return result.MapToDto();
    }
}