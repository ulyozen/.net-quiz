namespace Quiz.Persistence.Entities;

public class QuestionEntity : BaseEntity
{
    public string Title { get; set; }
    
    public string QuestionTypeId { get; set; }
    
    public QuestionTypeEntity QuestionType { get; set; }
    
    public string TemplateId { get; set; }
    
    public TemplateEntity Template { get; set; }
}