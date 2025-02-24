namespace Quiz.Persistence.Entities;

public class QuestionEntity : BaseEntity
{
    public string Title { get; set; }
    
    public QuestionTypeEntity QuestionType { get; set; }
    
    public List<string>? Options { get; set; }
    
    public string? CorrectAnswers { get; set; }
    
    public string TemplateId { get; set; }
    
    public TemplateEntity Template { get; set; }
}