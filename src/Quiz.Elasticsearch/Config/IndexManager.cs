using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;
using Microsoft.Extensions.Logging;
using Quiz.Elasticsearch.Entities;

namespace Quiz.Elasticsearch.Config;

public class IndexManager
{
    private readonly ElasticsearchClient _client;
    
    private readonly ILogger<IndexManager> _logger;
    
    private const string IndexName = "templates";
    
    private const string CustomAnalyzer = "custom_analyzer";
    
    public IndexManager(ElasticsearchClient client, ILogger<IndexManager> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task CreateIndexAsync()
    {
        try
        {
            var indexExists = await _client.Indices.ExistsAsync(IndexName);
            
            if (indexExists.Exists)
            {
                _logger.LogInformation("Elasticsearch index '{IndexName}' already exists.", IndexName);
                return;
            }
            
            var descriptor = new CreateIndexRequestDescriptor(IndexName)
                .Settings(s => s
                    .NumberOfShards(1)
                    .NumberOfReplicas(0)
                    .Analysis(a => a
                        .Analyzers(an => an
                            .Custom(CustomAnalyzer, ca => ca
                                .Tokenizer("standard")
                                .Filter(["lowercase", "russian_stemmer", "english_stemmer"])
                            )
                        )
                        .TokenFilters(tf => tf
                            .Stemmer("russian_stemmer", sf => sf.Language("russian"))
                            .Stemmer("english_stemmer", sf => sf.Language("english"))
                        )
                    )
                )
                .Mappings(m => m
                    .Properties(new Properties<TemplateIndex>
                        {
                            { "templateTitle", new TextProperty { Analyzer = CustomAnalyzer } },
                            { "templateDescription", new TextProperty { Analyzer = CustomAnalyzer } },
                            { "templateTopic", new KeywordProperty() },
                            { "templateTags", new KeywordProperty() },
                            { "questionTitle", new TextProperty { Analyzer = CustomAnalyzer } },
                            { "questionDescription", new TextProperty { Analyzer = CustomAnalyzer } },
                            
                            { "templateId", new KeywordProperty { Index = false } },
                            { "templateIsPublic", new BooleanProperty { Index = false } }
                        }
                    )
                );
            
            var createIndexResponse = await _client.Indices.CreateAsync(descriptor);
            if (createIndexResponse.IsValidResponse)
            {
                _logger.LogInformation("Created Elasticsearch index: {IndexName}", IndexName);
            }
            else
            {
                _logger.LogError("Failed to create index: {IndexName}. Debug Info: {DebugInfo}", IndexName, createIndexResponse.DebugInformation);
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Elasticsearch connection error.");
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Elasticsearch request timed out.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected Elasticsearch error.");
        }
    }
}