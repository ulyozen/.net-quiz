using Quiz.Core.Abstractions;
using Quiz.Core.ValueObjects;

namespace Quiz.Core.DomainEvents;

public class UserCreatedEvent : IDomainEvent
{
    public Email Email { get; private set; }
    
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    
    private UserCreatedEvent(Email email) => Email = email;

    public static UserCreatedEvent Create(Email email) => new(email);
}