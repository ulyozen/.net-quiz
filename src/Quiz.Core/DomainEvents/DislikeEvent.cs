using Quiz.Core.Abstractions;

namespace Quiz.Core.DomainEvents;

public class DislikeEvent : IDomainEvent
{
    public string TemplateId { get; private set; }
    
    public string UserId { get; private set; }
    
    private DislikeEvent(string templateId, string userId)
    {
        TemplateId = templateId;
        UserId = userId;
    }
    
    public static DislikeEvent Create(string templateId, string userId) => new(templateId, userId);
}