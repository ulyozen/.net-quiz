namespace Quiz.Core.ValueObjects;

public class TemplateMetadata
{
    private readonly List<string> _tags;
    
    private readonly List<string>? _allowedUsers;
    
    public string Title { get; }
    
    public string Description { get; }
    
    public string Topic { get; }
    
    public bool IsPublic { get; }
    
    public IReadOnlyList<string> Tags => _tags;
    
    public IReadOnlyList<string>? AllowedUsers => _allowedUsers;
    
    private TemplateMetadata(string title, string description, string topic, bool isPublic, 
        List<string> tags, List<string> allowedUsers)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentException.ThrowIfNullOrWhiteSpace(topic);
        
        Title = title;
        Description = description;
        Topic = topic;
        IsPublic = isPublic;
        
        _tags = tags;
        _allowedUsers = allowedUsers;
    }
    
    public static TemplateMetadata Create(string title, string description, string topic, bool isPublic,
        List<string> tags, List<string> allowedUsers)
    {
        return new TemplateMetadata(title, description, topic, isPublic, tags, allowedUsers);
    }
}