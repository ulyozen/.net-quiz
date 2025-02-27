namespace Quiz.Core.ValueObjects;

public class Dislike
{
    public string TemplateId { get; }
    
    public string UserId { get; }
    
    private Dislike(string templateId, string userId)
    {
        TemplateId = templateId;
        UserId     = userId;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not Dislike like) return false;
        
        return TemplateId == like.TemplateId && UserId == like.UserId;
    }
    
    public override int GetHashCode() => HashCode.Combine(TemplateId, UserId);
    
    public static Dislike Create(string templateId, string userId) => new(templateId, userId);
}