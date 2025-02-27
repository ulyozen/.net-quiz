using Quiz.Core.Abstractions;

namespace Quiz.Core.DomainEvents;

public class UserLoggedInEvent : IDomainEvent
{
    public string UserId { get; private set; }
    
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    
    private UserLoggedInEvent(string userId) => UserId = userId;
    
    public static UserLoggedInEvent Create(string userId) => new(userId);
}