namespace Quiz.Core.Entities;

public class Comment : BaseEntity
{
    public string TemplateId { get; set; }
    
    public string UserId { get; private set; }
    
    public string Username { get; private set; }
    
    public string Content { get; private set; }
    
    public DateTime CreateAt { get; private set; }
    
    private Comment() { }
    
    private Comment(string templateId, string userId, string username, string content)
    {
        TemplateId = templateId;
        UserId = userId;
        Username = username;
        Content = content;
        CreateAt = DateTime.UtcNow;
    }
    
    public static Comment Create(string templateId, string userId, string username, string text)
    {
        return new Comment(templateId, userId, username, text);
    }
    
    public static Comment Restore(string id, string userId, string username, 
        string text, DateTime createdAt)
    {
        return new Comment
        {
            Id = id,
            UserId = userId,
            Username = username,
            Content = text,
            CreateAt = createdAt
        };
    }
}