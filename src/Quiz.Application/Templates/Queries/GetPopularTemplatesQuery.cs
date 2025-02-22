using MediatR;
using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Application.Templates.Queries;

public class GetPopularTemplatesQuery : IRequest<PaginationResult<Template>>
{
    
}