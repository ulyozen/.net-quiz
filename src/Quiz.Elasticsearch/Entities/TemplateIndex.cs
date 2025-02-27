namespace Quiz.Elasticsearch.Entities;

public class TemplateIndex
{
    public string TemplateId { get; set; }
    
    public string TemplateTitle { get; set; }
    
    public string TemplateDescription { get; set; }
    
    public string TemplateTopic { get; set; }
    
    public bool TemplateIsPublic { get; set; }
    
    public HashSet<string> TemplateTags { get; set; }
    
    public ICollection<string> QuestionTitle { get; set; }
    
    public ICollection<string> QuestionDescription { get; set; }
}