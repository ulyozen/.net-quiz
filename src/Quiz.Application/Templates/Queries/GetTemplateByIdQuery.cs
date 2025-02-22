using MediatR;
using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Application.Templates.Queries;

public class GetTemplateByIdQuery : IRequest<OperationResult<Template>>
{
    public string TemplateId { get; set; }
}