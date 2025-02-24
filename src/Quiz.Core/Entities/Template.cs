using Quiz.Core.Abstractions;
using Quiz.Core.DomainEvents;
using Quiz.Core.ValueObjects;

namespace Quiz.Core.Entities;

public class Template : BaseEntity, IHasDomainEvent
{
    private readonly List<IDomainEvent> _domainEvents = new();
    
    private readonly List<Question> _questions = new();
    
    private readonly List<Comment> _comments = new();
    
    private readonly List<Dislike> _dislikes = new();
    
    private readonly List<Like> _likes = new();
    
    public TemplateMetadata TemplateMetadata { get; private set; }
    
    public string AuthorId { get; private set; }
    
    public string AuthorName { get; private set; }
    
    public string ImageUrl { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime? UpdatedAt { get; private set; }
    
    public IReadOnlyList<Question> Questions => _questions;
    
    public IReadOnlyList<Comment> Comments => _comments;
    
    public IReadOnlyList<Dislike> Dislikes => _dislikes;
    
    public IReadOnlyList<Like> Likes => _likes;
    
    private Template(string templateId) => Id = templateId;
    
    private Template(string templateId, List<Question> questions)
    { 
        Id = templateId;
        
        _questions.AddRange(questions);
    } 
    
    private Template(TemplateMetadata metadata, string authorId, string authorName, string imageUrl, 
        DateTime createdAt, List<Question> questions)
    {
        TemplateMetadata = metadata;
        AuthorId = authorId;
        AuthorName = authorName;
        ImageUrl = imageUrl;
        CreatedAt = createdAt;
        
        _questions = questions;
        
        _domainEvents.Add(TemplateEvent.Create(metadata.Title));
    }
    
    public void LikeTemplate(string templateId, string userId)
    {
        var like = _likes.Find(l => l.UserId == userId);
        if (like == null)
        {
            _likes.Add(Like.Create(templateId, userId));
        }
        else
        {
            _likes.Remove(like);
        }
        
        _domainEvents.Add(LikeEvent.Create(userId, templateId));
    }
    
    public void DislikeTemplate(string templateId, string userId)
    {
        var dislike = _dislikes.FirstOrDefault(l => l.UserId == userId);
        if (dislike == null)
        {
            _dislikes.Add(Dislike.Create(templateId, userId));
        }
        else
        {
            _dislikes.Remove(dislike);
        }
        
        _domainEvents.Add(DislikeEvent.Create(userId, templateId));
    }
    
    public void UpdateTemplate(TemplateMetadata metadata)
    {
        TemplateMetadata = metadata;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void AddQuestion(Question question)
    {
        _questions.Add(question);
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void RemoveQuestion(Question question)
    {
        _questions.Remove(question);
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
        
        _domainEvents.Add(CommentEvent.Create(Id, comment.UserId, comment.Content));
    }
    
    public void RemoveComment(Comment comment)
    {
        _comments.Remove(comment);
        
        _domainEvents.Add(CommentEvent.Create(Id, comment.UserId, comment.Content));
    }
    
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents;
    
    public void ClearDomainEvents() => _domainEvents.Clear();
    
    public static Template Create(TemplateMetadata metadata, string authorId, string authorName, 
        string imageUrl, DateTime createdAt, List<Question> questions)
    {
        return new Template(metadata, authorId, authorName, imageUrl, createdAt, questions);
    }
    
    public static Template Restore(string templateId, TemplateMetadata metadata)
    {
        return new Template(templateId)
        {
            TemplateMetadata = metadata
        };
    }
    
    public static Template Restore(string templateId, TemplateMetadata metadata, string authorId, string authorName, string imageUrl, 
        DateTime createdAt, DateTime? updatedAt, List<Question> questions)
    {
        return new Template(templateId, questions)
        {
            TemplateMetadata = metadata,
            AuthorId = authorId,
            AuthorName = authorName,
            ImageUrl = imageUrl,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
        };
    }
}