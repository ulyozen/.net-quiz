using Quiz.Core.Common;
using Quiz.Core.Entities;

namespace Quiz.Core.Repositories;

public interface ITemplateRepository
{
    Task<OperationResult<Template>> GetByIdAsync(string templateId);
    
    Task<PaginationResult<Template>> GetTemplatesAsync(int page, int pageSize);
    
    Task<IEnumerable<Template>> GetPopularTemplatesAsync(int totalTemp = 5);
    
    Task<OperationResult> AddAsync(Template template);
    
    Task<OperationResult> UpdateAsync(Template template);
    
    Task<OperationResult> DeleteAsync(string templateId);
}