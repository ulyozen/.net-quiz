namespace Quiz.Persistence.Entities;

public class TemplateEntity : FullAuditEntity
{
    public string AuthorId { get; set; }
    
    public string AuthorName { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public string Topic { get; set; }
    
    public bool IsPublic { get; set; }
    
    public string ImageUrl { get; set; }
    
    public UserEntity UserEntity { get; set; }
    
    public IList<LikeEntity> Likes { get; set; }
    
    public IList<CommentEntity> Comments { get; set; }
    
    public IList<QuestionEntity> Questions { get; set; }
    
    public IList<SubmissionEntity> Submissions { get; set; }
    
    public IList<AllowedUsers> AllowedUsers { get; set; }
    
    public IList<TemplateTag> TemplateTags { get; set; }
}