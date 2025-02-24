using MediatR;
using Quiz.Core.Common;

namespace Quiz.Application.Templates.Commands;

public class CreateTemplateCommand : IRequest<OperationResult>
{
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public string Topic { get; set; }
    
    public string AuthorId { get; set; }
    
    public string AuthorName { get; set; }
    
    public string ImageUrl { get; set; }
    
    public bool IsPublic { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public List<string>? Tags { get; set; }
    
    public List<string>? AllowedUsers { get; set; }
    
    
}