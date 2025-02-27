namespace Quiz.Core.ValueObjects;

public class Like
{
    public string TemplateId { get; }
    
    public string UserId { get; }
    
    private Like(string templateId, string userId)
    {
        TemplateId = templateId;
        UserId     = userId;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not Like like) return false;
        
        return TemplateId == like.TemplateId && UserId == like.UserId;
    }
    
    public override int GetHashCode() => HashCode.Combine(TemplateId, UserId);
    
    public static Like Create(string templateId, string userId) => new(templateId, userId);
}