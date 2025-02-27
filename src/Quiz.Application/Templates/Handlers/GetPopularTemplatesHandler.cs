using MediatR;
using Quiz.Application.Mappers;
using Quiz.Application.Templates.Dtos;
using Quiz.Application.Templates.Queries;
using Quiz.Core.Entities;
using Quiz.Core.Repositories;

namespace Quiz.Application.Templates.Handlers;

public class GetPopularTemplatesHandler : IRequestHandler<GetPopularTemplatesQuery, IList<TemplateDto>>
{
    private readonly ITemplateRepository _templateRepository;
    
    public GetPopularTemplatesHandler(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }
    
    public async Task<IList<TemplateDto>> Handle(GetPopularTemplatesQuery request, CancellationToken cancellationToken)
    {
        var result = await _templateRepository.GetPopularTemplatesAsync();
        
        return result.MapToDto();
    }
}