using MediatR;
using Quiz.Application.Templates.Commands;
using Quiz.Core.Common;
using Quiz.Core.Repositories;

namespace Quiz.Application.Templates.Handlers;

public class DeleteTemplateHandler : IRequestHandler<DeleteTemplateCommand, OperationResult>
{
    private readonly ITemplateRepository _templateRepository;

    public DeleteTemplateHandler(ITemplateRepository templateRepository)
    {
        _templateRepository = templateRepository;
    }
    
    public async Task<OperationResult> Handle(DeleteTemplateCommand command, CancellationToken cancellationToken)
    {
        return await _templateRepository.DeleteAsync(command.TemplateId);
    }
}