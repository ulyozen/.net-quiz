using Elastic.Clients.Elasticsearch;
using Quiz.Core.Entities;
using Quiz.Core.Repositories;

namespace Quiz.Elasticsearch.Repositories;

public class ElasticsearchRepository(ElasticsearchClient client) : ISearchRepository
{
    private const string IndexName = "templates";
    
    public async Task<List<Template>> SearchTemplatesAsync(string query, int page = 1, int pageSize = 10)
    {
        var fields = new[]
        {
            "title",
            "description",
            "question.title",
            "question.description",
            "comments.text"
        };
        
        var response = await client.SearchAsync<Template>(s  => s
            .Index(IndexName)
            .Query(q => q
                .MultiMatch(m => m
                    .Fields(fields)
                    .Query(query)
                    .Fuzziness(new Fuzziness("AUTO"))
                )
            )
            .From((page - 1) * pageSize)
            .Size(pageSize)
        );
        
        return response.Documents.ToList();
    }

    public async Task AddOrUpdateAsync(Template template)
    {
        await client.IndexAsync(template, idx => idx.Index(IndexName));
    }

    public async Task DeleteAsync(string templateId)
    {
        await client.DeleteAsync(templateId, idx => idx.Index(IndexName));
    }
}