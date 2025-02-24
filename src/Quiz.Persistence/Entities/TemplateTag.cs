using Quiz.Core.Abstractions;

namespace Quiz.Persistence.Entities;

public class TemplateTag : IManyToMany
{
    public string TemplateId { get; set; }
    
    public TemplateEntity Template { get; set; }
    
    public string TagId { get; set; }
    
    public TagEntity Tag { get; set; }
}