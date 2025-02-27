using Quiz.Core.Abstractions;

namespace Quiz.Core.DomainEvents;

public class LikeEvent : IDomainEvent
{
    public string TemplateId { get; private set; }
    
    public string UserId { get; private set; }
    
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    
    private LikeEvent(string templateId, string userId)
    {
        TemplateId = templateId;
        UserId = userId;
    }
    
    public static LikeEvent Create(string templateId, string userId) => new(templateId, userId);
}