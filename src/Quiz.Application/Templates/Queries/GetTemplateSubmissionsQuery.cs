using MediatR;
using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Application.Templates.Queries;

public class GetTemplateSubmissionsQuery : IRequest<OperationResult<Submission>>
{
    public string TemplateId { get; set; }
}