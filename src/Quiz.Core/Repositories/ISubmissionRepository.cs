using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Core.Repositories;

public interface ISubmissionRepository
{
    Task<IEnumerable<Submission>> GetByTemplateIdAsync(string templateId);

    Task<IEnumerable<Submission>> GetByUserIdAsync(string userId);
    
    Task<OperationResult> AddSubmissionAsync(Submission submission);

    Task<OperationResult> DeleteSubmissionAsync(string submissionId);
}