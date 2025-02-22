using Microsoft.EntityFrameworkCore;
using Quiz.Core.Common;
using Quiz.Core.Entities;
using Quiz.Core.Repositories;
using Quiz.Persistence.Context;
using Quiz.Persistence.Mappers;

namespace Quiz.Persistence.Repositories;

public class CommentRepository(AppDbContext context) : ICommentRepository
{
    public async Task<IEnumerable<Comment>> GetCommentsByTemplateAsync(string templateId, int page, int pageSize)
    {
        return (await context.Comments
            .Where(c => c.TemplateId == templateId)
            .OrderBy(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(c => c.User)
            .ToListAsync())
            .MapToComments();
    }
    
    public async Task<OperationResult<Comment>> AddCommentAsync(Comment comment)
    {
        await context.Comments.AddAsync(comment.MapToEntity());
        
        return OperationResult<Comment>.SuccessResult(comment);
    }
    
    public async Task<OperationResult> DeleteCommentAsync(string commentId)
    {
        var comment = await context.Comments.FindAsync(commentId);
        if (comment is null)
            return OperationResult.Failure("Comment not found");
        
        context.Comments.Remove(comment);
        
        return OperationResult.SuccessResult();
    }
}