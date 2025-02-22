using Quiz.Core.Common;

namespace Quiz.Core.Repositories;

public interface ILikeRepository
{
    Task<OperationResult> AddOrRemoveLikeAsync(string templateId, string userId);
    
    Task<int> GetLikeCountAsync(string templateId);
}