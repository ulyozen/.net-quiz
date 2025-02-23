namespace Quiz.Application.Templates.Dtos;

public class PopularTemplate
{
    public string TemplateId { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public string Topic { get; set; }
    
    public bool IsPublic { get; set; }
    
    public IReadOnlyList<string> Tags { get; set; }
}