using Microsoft.EntityFrameworkCore;
using Quiz.Core.Common;
using Quiz.Core.Repositories;
using Quiz.Persistence.Context;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Repositories;

public class LikeRepository(AppDbContext context) : ILikeRepository
{
    public async Task<OperationResult> AddOrRemoveLikeAsync(string templateId, string userId)
    {
        var like = await context.Likes.FindAsync(templateId, userId);
        
        if (like is null)
        {
            await context.Likes.AddAsync(new LikeEntity { TemplateId = templateId, UserId = userId });
        }
        else
        {
            context.Likes.Remove(like);
        }
        
        return OperationResult.SuccessResult();
    }
    
    public async Task<int> GetLikeCountAsync(string templateId)
    {
        return await context.Likes.CountAsync(l => l.TemplateId == templateId);
    }
}