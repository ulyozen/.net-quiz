using Quiz.Core.Abstractions;

namespace Quiz.Core.DomainEvents;

public class SubmissionEvent : IDomainEvent
{
    public string TemplateId { get; private set; }
    
    public string UserId { get; private set; }
    
    public DateTime CreatedAt { get; } = DateTime.UtcNow;

    private SubmissionEvent(string templateId, string userId)
    {
        TemplateId = templateId;
        UserId = userId;
    }
    
    public static SubmissionEvent Create(string templateId, string userId) => new(templateId, userId);
}