using Microsoft.EntityFrameworkCore;
using Quiz.Core.Common;
using Quiz.Core.Entities;
using Quiz.Core.Repositories;
using Quiz.Persistence.Context;
using Quiz.Persistence.Mappers;

namespace Quiz.Persistence.Repositories;

public class SubmissionRepository(AppDbContext context) : ISubmissionRepository
{
    public async Task<IEnumerable<Submission>> GetByTemplateIdAsync(string templateId)
    {
        return (await context.Submissions
            .Where(s => s.TemplateId == templateId)
            .ToListAsync())
            .MapToSubmissions();
    }
    
    public async Task<IEnumerable<Submission>> GetByUserIdAsync(string userId)
    {
        return (await context.Submissions
            .Where(s => s.UserId == userId)
            .ToListAsync())
            .MapToSubmissions();
    }
    
    public async Task<OperationResult> AddSubmissionAsync(Submission submission)
    {
        await context.Submissions.AddAsync(submission.MapToEntity());
        
        return OperationResult.SuccessResult();
    }
    
    public async Task<OperationResult> DeleteSubmissionAsync(string submissionId)
    {
        var submission = await context.Submissions.FindAsync(submissionId);
        if (submission is null)
            return OperationResult.Failure("Submission not found");
        
        context.Submissions.Remove(submission);
        
        return OperationResult.SuccessResult();
    }
}