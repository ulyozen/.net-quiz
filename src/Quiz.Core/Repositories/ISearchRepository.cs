using Quiz.Core.Entities;

namespace Quiz.Core.Repositories;

public interface ISearchRepository
{
    Task<IList<Template>> SearchTemplatesAsync(string query, int page = 1, int pageSize = 10);
    
    Task AddTemplateAsync(Template template);
}