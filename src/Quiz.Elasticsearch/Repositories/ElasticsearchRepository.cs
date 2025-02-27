using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using Quiz.Application.Templates.Dtos;
using Quiz.Core.Entities;
using Quiz.Core.Repositories;
using Quiz.Elasticsearch.Entities;
using Quiz.Elasticsearch.Mappers;

namespace Quiz.Elasticsearch.Repositories;

public class ElasticsearchRepository(ElasticsearchClient client) : ISearchRepository
{
    private const string IndexName = "templates";
    
    public async Task<IList<Template>> SearchTemplatesAsync(string query, int page = 1, int pageSize = 10)
    {
        var fields = new[]
        {
            "templateTitle",
            "templateDescription",
            "templateTopic",
            "templateTags",
            "questionTitle",
            "questionDescription",
        };
            
        var response = await client.SearchAsync<TemplateIndex>(s  => s
            .Index(IndexName)
            .Query(q => q
                .MultiMatch(m => m
                    .Fields(fields)
                    .Query(query)
                    .Fuzziness(new Fuzziness("Auto"))
                )
            )
            .From((page - 1) * pageSize)
            .Size(pageSize)
        );
        
        var results = response.Documents.ToList().MapToTemplates();
        
        return results;
    }
    
    public async Task AddTemplateAsync(Template template)
    {
        await client.IndexAsync(template.MapToIndex(), idx => idx.Index(IndexName));
    }
    
    public async Task UpdateTemplateAsync(Template template)
    {
        var templateIndex = new TemplateIndex();
        
        await client.IndexAsync(templateIndex, idx => idx.Index(IndexName));
    }
    
    public async Task DeleteAsync(string templateId)
    {
        await client.DeleteAsync(templateId, idx => idx.Index(IndexName));
    }
}