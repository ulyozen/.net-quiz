using Quiz.Core.Abstractions;

namespace Quiz.Core.DomainEvents;

public class CommentEvent : IDomainEvent
{
    public string TemplateId { get; private set; }
    
    public string UserId { get; private set; }
    
    public string Text { get; private set; }
    
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    
    private CommentEvent(string templateId, string userId, string text)
    {
        TemplateId = templateId;
        UserId     = userId;
        Text       = text;
    }
    
    public static CommentEvent Create(string templateId, string userId, string text)
    {
        return new CommentEvent(templateId, userId, text);
    }
}