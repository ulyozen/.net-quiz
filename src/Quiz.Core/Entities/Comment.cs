namespace Quiz.Core.Entities;

public class Comment : Entity
{
    public string TemplateId { get; set; } 
    
    public string UserId { get; private set; }
    
    public string Username { get; private set; }
    
    public string Content { get; private set; }
    
    public DateTime CreateAt { get; private set; }
    
    private Comment(string id, string templateId, string userId, string username, 
        string content, DateTime createdAt) : base(id)
    {
        TemplateId = templateId;
        UserId     = userId;
        Username   = username;
        Content    = content;
        CreateAt   = createdAt;
    }
    
    private Comment(string id, string userId, string username, string content, DateTime createdAt) : base(id)
    {
        UserId   = userId;
        Username = username;
        Content  = content;
        CreateAt = createdAt;
    }
    
    public static Comment Create(string id, string templateId, string userId, 
        string username, string text, DateTime createdAt)
    {
        return new Comment(id, templateId, userId, username, text, createdAt);
    }
    
    public static Comment Restore(string id, string userId, string username, string text, DateTime createdAt)
    {
        return new Comment(id, userId, username, text, createdAt);
    }
}