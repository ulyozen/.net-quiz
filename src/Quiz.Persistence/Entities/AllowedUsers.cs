using Quiz.Core.Abstractions;

namespace Quiz.Persistence.Entities;

public class AllowedUsers : IManyToManyEntity
{
    public string UserId { get; set; }
    
    public UserEntity User { get; set; }
    
    public string TemplateId { get; set; }
    
    public TemplateEntity Template { get; set; }
}