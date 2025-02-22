using Quiz.Core.Abstractions;
using Quiz.Core.DomainEnums;
using Quiz.Core.DomainEvents;
using Quiz.Core.ValueObjects;

namespace Quiz.Core.Entities;

public class User : BaseEntity, IHasDomainEvent
{
    private readonly List<IDomainEvent> _domainEvents = new();
    
    public string Username { get; private set; }
    
    public Email Email { get; private set; }
    
    public string Password { get; private set; }
    
    public string Role { get; private set; }
    
    public bool IsBlocked { get; private set; }
    
    public bool RememberMe { get; private set; }
    
    private User(string userId)
    {
        Id = userId;
        
        _domainEvents.Add(UserLoggedInEvent.Create(userId));
    }

    private User(string username, Email email, string password, string role, bool rememberMe)
    {
        Username = username;
        Email = email;
        Password = password;
        Role = role;
        IsBlocked = false;
        RememberMe = rememberMe;
        
        _domainEvents.Add(UserCreatedEvent.Create(email));
    }
    
    public void Block() => IsBlocked = true;
    
    public void Unblock() => IsBlocked = false;
    
    public void ChangeRole(string role) => Role = role;
    
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents;
    
    public void ClearDomainEvents() => _domainEvents.Clear();
    
    public static User Create(string username, Email email, string password, bool rememberMe = false)
    {
        return new User(username, email, password, GetRole(email), rememberMe);
    }
    
    public static User Restore(string userId, string username, string email, bool isBlocked, bool rememberMe)
    {
        return new User(userId)
        {
            Username = username,
            Email = Email.From(email),
            IsBlocked = isBlocked,
            RememberMe = rememberMe
        };
    }
    
    public static User Restore(string userId, string username, string email, bool isBlocked, List<string> role)
    {
        return new User(userId)
        {
            Username = username,
            Email = Email.From(email),
            IsBlocked = isBlocked,
            Role = string.Join(", ", role)
        };
    }
    
    private static string GetRole(Email email)
    {
        return email.Value.Contains("admin", StringComparison.OrdinalIgnoreCase) ? "admin" : "user";
    }
}

