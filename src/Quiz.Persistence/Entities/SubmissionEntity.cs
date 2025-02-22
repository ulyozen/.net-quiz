namespace Quiz.Persistence.Entities;

public class SubmissionEntity : AuditEntity
{
    public List<AnswerEntity> Answers { get; set; }
    
    public string TemplateId { get; set; }
    
    public TemplateEntity Template { get; set; }
    
    public string UserId { get; set; }
    
    public UserEntity User { get; set; }
}