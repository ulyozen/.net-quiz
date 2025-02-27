namespace Quiz.Core.ValueObjects;

public class TemplateMetadata
{
    private readonly HashSet<string> _tags;
    
    private readonly HashSet<string> _allowedUsers;
    
    public string Title { get; }
    
    public string Description { get; }
    
    public string Topic { get; }
    
    public bool IsPublic { get; }
    
    public IReadOnlySet<string> Tags => _tags;
    
    public IReadOnlySet<string> AllowedUsers => _allowedUsers;
    
    private TemplateMetadata(string title, string description, string topic, bool isPublic, 
        HashSet<string> tags)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentException.ThrowIfNullOrWhiteSpace(topic);
        
        Title = title;
        Description = description;
        Topic = topic;
        IsPublic = isPublic;
        
        _tags = tags;
    }
    
    private TemplateMetadata(string title, string description, string topic, bool isPublic, 
        HashSet<string> tags, HashSet<string> allowedUsers)
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

    public override bool Equals(object? obj)
    {
        if (obj is not TemplateMetadata temp) return false;
        
        return Title       == temp.Title       &&
               Description == temp.Description &&
               Topic       == temp.Topic       &&
               IsPublic    == temp.IsPublic    &&
               _tags
                   .SetEquals(temp._tags)      &&
               _allowedUsers
                   .SetEquals(temp._allowedUsers);
    }
    
    public override int GetHashCode()
    {
        var hash = HashCode.Combine(Title, Description, Topic, IsPublic);
        
        foreach (var tag in _tags)
        {
            hash = HashCode.Combine(hash, tag);
        }

        foreach (var user in _allowedUsers)
        {
            hash = HashCode.Combine(hash, user);
        }
        
        return hash;
    }
    
    public static TemplateMetadata Create(string title, string description, string topic, bool isPublic, 
        HashSet<string> tags)
    {
        return new TemplateMetadata(title, description, topic, isPublic, tags);
    }
    
    public static TemplateMetadata Create(string title, string description, string topic, bool isPublic,
        HashSet<string> tags, HashSet<string> allowedUsers)
    {
        return new TemplateMetadata(title, description, topic, isPublic, tags, allowedUsers);
    }
}