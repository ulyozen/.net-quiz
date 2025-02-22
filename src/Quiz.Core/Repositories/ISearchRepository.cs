using Quiz.Core.Entities;

namespace Quiz.Core.Repositories;

public interface ISearchRepository
{
    Task<List<Template>> SearchTemplatesAsync(string query, int page = 1, int pageSize = 10);
}