namespace Quiz.Core.Entities;

public class Like
{
    public string TemplateId { get; private set; }
    
    public string UserId { get; private set; }
    
    private Like(string templateId, string userId)
    {
        TemplateId = templateId;
        UserId = userId;
    }
    
    public static Like Create(string templateId, string userId) => new(templateId, userId);
}