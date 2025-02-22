using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Core.Repositories;

public interface ICommentRepository
{
    Task<IEnumerable<Comment>> GetCommentsByTemplateAsync(string templateId, int page, int pageSize);
    
    Task<OperationResult<Comment>> AddCommentAsync(Comment comment);
    
    Task<OperationResult> DeleteCommentAsync(string commentId);
}