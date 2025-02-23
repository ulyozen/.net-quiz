using MediatR;
using Quiz.Application.Mappers;
using Quiz.Application.Templates.Commands;
using Quiz.Core.Common;
using Quiz.Core.Repositories;

namespace Quiz.Application.Templates.Handlers;

public class CreateTemplateHandler : IRequestHandler<CreateTemplateCommand, OperationResult>
{
    private readonly ITemplateRepository _templateRepository;

    public CreateTemplateHandler(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }
    
    public Task<OperationResult> Handle(CreateTemplateCommand command, CancellationToken cancellationToken)
    {
        var temp = command.MapToTemplate();

        var result = _templateRepository.AddAsync(temp);

        return result;
    }
}