using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Core.Repositories;

public interface ITemplateRepository
{
    Task<OperationResult<Template>> GetByIdAsync(string templateId);
    
    Task<IEnumerable<Template>> GetByUserIdAsync(string userId);
    
    Task<IEnumerable<Template>> GetPublicTemplatesAsync(int page, int pageSize, bool isPublic = true);
    
    Task<IEnumerable<Template>> GetPopularTemplatesAsync(int countTemp = 5);
    
    Task<OperationResult> AddAsync(Template template);
    
    Task<OperationResult> UpdateAsync(Template template);
    
    Task<OperationResult> DeleteAsync(string templateId);
}