using MediatR;
using Quiz.Core.Common;

namespace Quiz.Application.Templates.Commands;

public class DeleteTemplateCommand : IRequest<OperationResult>
{
    public string TemplateId { get; set; }
}