using MediatR;
using Quiz.Application.Abstractions;
using Quiz.Application.Mappers;
using Quiz.Application.Templates.Commands;
using Quiz.Core.Common;
using Quiz.Core.Repositories;

namespace Quiz.Application.Templates.Handlers;

public class CreateTemplateHandler : IRequestHandler<CreateTemplateCommand, OperationResult>
{
    private readonly ITemplateRepository _templateRepository;
    private readonly ISearchRepository _searchRepository;
    private readonly IGuidFactory _guidFactory;

    public CreateTemplateHandler(
        ITemplateRepository templateRepository, 
        ISearchRepository searchRepository, 
        IGuidFactory guidFactory)
    {
        _templateRepository = templateRepository;
        _searchRepository = searchRepository;
        _guidFactory = guidFactory;
    }
    
    public async Task<OperationResult> Handle(CreateTemplateCommand command, CancellationToken cancellationToken)
    {
        var template = command.MapToTemplate(_guidFactory);
        
        var result = await _templateRepository.AddAsync(template);

        if (result.Success) await _searchRepository.AddTemplateAsync(template);

        return result;
    }
}