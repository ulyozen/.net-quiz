using MediatR;
using Quiz.Application.Templates.Dtos;

namespace Quiz.Application.Templates.Queries;

public class GetPopularTemplatesQuery : IRequest<IEnumerable<PopularTemplate>>;