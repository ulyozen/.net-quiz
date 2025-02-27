using Quiz.Core.Abstractions;
using Quiz.Core.DomainEnums;
using Quiz.Core.DomainEvents;
using Quiz.Core.ValueObjects;

namespace Quiz.Core.Entities;

public class User : AggregateRoot
{
    public string Username { get; private set; }
    
    public Email Email { get; private set; }
    
    public string Password { get; private set; }
    
    public string Role { get; private set; }
    
    public bool IsBlocked { get; private set; }
    
    public bool RememberMe { get; private set; }
    
    private User(string id) : base(id) { }
    
    private User(string id, string username, Email email, string password, string role) : base(id)
    {
        Username = username;
        Email    = email;
        Password = password;
        Role     = role;
        
        RaiseDomainEvent(UserCreatedEvent.Create(email));
    }
    
    private User(string id, string username, string email, bool isBlocked, bool rememberMe) : base(id)
    {
        Username   = username;
        Email      = Email.From(email);
        IsBlocked  = isBlocked;
        RememberMe = rememberMe;
        
        RaiseDomainEvent(UserLoggedInEvent.Create(id));
    }
    
    public void Block() => IsBlocked = true;
    
    public void Unblock() => IsBlocked = false;
    
    public void ChangeRole(string role) => Role = role;
    
    public static User Create(string id, string username, string email, string password)
    {
        var emailValue = Email.From(email);
        
        return new User(id, username, emailValue, password, GetRole(emailValue));
    }
    
    public static User Restore(string userId, string username, string email, bool isBlocked, bool rememberMe)
    {
        return new User(userId, username, email, isBlocked, rememberMe);
    }
    
    public static User Restore(string userId, string username, string email, bool isBlocked, List<string> role)
    {
        return new User(userId)
        {
            Username  = username,
            Email     = Email.From(email),
            IsBlocked = isBlocked,
            Role      = string.Join(", ", role)
        };
    }
    
    private static string GetRole(Email email)
    {
        return email.Value.Contains("admin", StringComparison.OrdinalIgnoreCase) ? "admin" : "user";
    }
}

