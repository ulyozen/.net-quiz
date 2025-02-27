namespace Quiz.Application.Templates.Dtos;

public class TemplateDto
{
    public string TemplateId { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public string Topic { get; set; }
    
    public bool IsPublic { get; set; }
    
    public HashSet<string> Tags { get; set; }
}