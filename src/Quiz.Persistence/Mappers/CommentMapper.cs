using Quiz.Core.Entities;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Mappers;

public static class CommentMapper
{
    public static CommentEntity MapToEntity(this Comment comment)
    {
        return new CommentEntity
        {
            Id = comment.Id,
            Content = comment.Content,
            UserId = comment.UserId,
            TemplateId = comment.TemplateId,
            CreatedAt = comment.CreateAt,
            CreatedBy = comment.UserId
        };
    }
    
    public static IEnumerable<Comment> MapToComments(this IEnumerable<CommentEntity> entities)
    {
        return entities.Select(entity => entity.MapToComment());
    }
    
    private static Comment MapToComment(this CommentEntity entity)
    {
        return Comment.Restore(entity.Id, entity.UserId, entity.User.Name, entity.Content, entity.CreatedAt);
    }
}