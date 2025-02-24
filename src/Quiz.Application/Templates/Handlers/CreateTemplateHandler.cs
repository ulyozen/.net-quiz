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
    private readonly IGuidFactory _guidFactory;

    public CreateTemplateHandler(ITemplateRepository templateRepository, IGuidFactory guidFactory)
    {
        _templateRepository = templateRepository;
        _guidFactory = guidFactory;
    }
    
    public async Task<OperationResult> Handle(CreateTemplateCommand command, CancellationToken cancellationToken)
    {
        return await _templateRepository.AddAsync(command.MapToTemplate(_guidFactory));
    }
}