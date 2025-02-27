using Quiz.Core.Abstractions;
using Quiz.Core.DomainEvents;
using Quiz.Core.ValueObjects;

namespace Quiz.Core.Entities;

public class Template : AggregateRoot
{
    private readonly List<Question> _questions  = new();
    
    private readonly List<Comment> _comments    = new();
    
    private readonly HashSet<Dislike> _dislikes = new();
    
    private readonly HashSet<Like> _likes       = new();
    
    public TemplateMetadata TemplateMetadata { get; private set; }
    
    public string AuthorId { get; private set; }
    
    public string AuthorName { get; private set; }
    
    public string ImageUrl { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime? UpdatedAt { get; private set; }
    
    public IReadOnlyList<Question> Questions => _questions;
    
    public IReadOnlyList<Comment> Comments   => _comments;
    
    public IReadOnlySet<Dislike> Dislikes    => _dislikes;
    
    public IReadOnlySet<Like> Likes          => _likes;

    public Template(string id) : base(id) { }
    
    private Template(string id, List<Question> questions) : base(id)
    { 
        _questions.AddRange(questions);
    } 
    
    private Template(string id, TemplateMetadata metadata, string authorId, string authorName, string imageUrl, 
        DateTime createdAt, List<Question> questions) : base(id)
    {
        TemplateMetadata = metadata;
        AuthorId         = authorId;
        AuthorName       = authorName;
        ImageUrl         = imageUrl;
        CreatedAt        = createdAt;
        
        _questions       = questions;
        
        RaiseDomainEvent(TemplateEvent.Create(metadata.Title));
    }
    
    public void LikeTemplate(string templateId, string userId)
    {
        var like = Like.Create(templateId, userId);
        
        if (!_likes.Add(like)) _likes.Remove(like);
        
        RaiseDomainEvent(LikeEvent.Create(templateId, userId));
    }
    
    public void DislikeTemplate(string templateId, string userId)
    {
        var dislike = Dislike.Create(templateId, userId);
        
        if (!_dislikes.Add(dislike)) _dislikes.Remove(dislike);
        
        RaiseDomainEvent(DislikeEvent.Create(templateId, userId));
    }
    
    public void UpdateTemplate(TemplateMetadata metadata)
    {
        TemplateMetadata = metadata;
        UpdatedAt        = DateTime.UtcNow;
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
        
        RaiseDomainEvent(CommentEvent.Create(Id, comment.UserId, comment.Content));
    }
    
    public void RemoveComment(Comment comment)
    {
        _comments.Remove(comment);
        
        RaiseDomainEvent(CommentEvent.Create(Id, comment.UserId, comment.Content));
    }
    
    public static Template Create(string id, TemplateMetadata metadata, string authorId, string authorName, 
        string imageUrl, DateTime createdAt, List<Question> questions)
    {
        return new Template(id, metadata, authorId, authorName, imageUrl, createdAt, questions);
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
            AuthorId         = authorId,
            AuthorName       = authorName,
            ImageUrl         = imageUrl,
            CreatedAt        = createdAt,
            UpdatedAt        = updatedAt,
        };
    }
}