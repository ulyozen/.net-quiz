namespace Quiz.Persistence.Entities;

public class LikeEntity
{
    public string TemplateId { get; set; }
    
    public TemplateEntity Template { get; set; }
    
    public string UserId { get; set; }
    
    public UserEntity User { get; set; }
}