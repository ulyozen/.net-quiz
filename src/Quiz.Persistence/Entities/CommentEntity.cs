namespace Quiz.Persistence.Entities;

public class CommentEntity : FullAuditEntity
{
    public string Content { get; set; }
    
    public string UserId { get; set; }
    
    public UserEntity User { get; set; }
    
    public string TemplateId { get; set; }
    
    public TemplateEntity Template { get; set; }
}