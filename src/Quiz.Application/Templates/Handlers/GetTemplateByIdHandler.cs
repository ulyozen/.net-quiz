using MediatR;
using Quiz.Application.Templates.Queries;
using Quiz.Core.Common;
using Quiz.Core.Entities;
using Quiz.Core.Repositories;

namespace Quiz.Application.Templates.Handlers;

public class GetTemplateByIdHandler : IRequestHandler<GetTemplateByIdQuery, OperationResult<Template>>
{
    private readonly ITemplateRepository _templateRepository;

    public GetTemplateByIdHandler(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }
    
    public async Task<OperationResult<Template>> Handle(GetTemplateByIdQuery query, CancellationToken cancellationToken)
    {
        var result = await _templateRepository.GetByIdAsync(query.TemplateId);
        
        return !result.Success 
            ? OperationResult<Template>.Failure(result.Errors)
            : OperationResult<Template>.SuccessResult(result.Data!);
    }
}