namespace Quiz.Core.Entities;

public class Dislike
{
    public string TemplateId { get; private set; }
    
    public string UserId { get; private set; }
    
    private Dislike(string templateId, string userId)
    {
        TemplateId = templateId;
        UserId = userId;
    }
    
    public static Dislike Create(string templateId, string userId) => new(templateId, userId);
}