using Quiz.Core.Abstractions;

namespace Quiz.Core.DomainEvents;

public class TemplateEvent : IDomainEvent
{
    public string Title { get; private set; }
    
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    
    private TemplateEvent(string title) => Title = title;
    
    public static TemplateEvent Create(string title) => new(title);
}