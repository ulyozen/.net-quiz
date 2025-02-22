using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;
using Microsoft.Extensions.Logging;
using Quiz.Core.Entities;
using Template = Quiz.Core.Entities.Template;

namespace Quiz.Elasticsearch.Common;

public class ElasticsearchIndexManager
{
    private readonly ElasticsearchClient _client;
    
    private readonly ILogger<ElasticsearchIndexManager> _logger;
    
    private const string IndexName = "templates";
    
    private const string CustomAnalyzer = "custom_analyzer";
    
    public ElasticsearchIndexManager(ElasticsearchClient client, ILogger<ElasticsearchIndexManager> logger)
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
                    .Properties(new Properties<Template>
                        {
                            { "id", new KeywordProperty() },
                            { "title", new TextProperty { Analyzer = CustomAnalyzer } },
                            { "description", new TextProperty { Analyzer = CustomAnalyzer } },
                            { "tags", new KeywordProperty() },
                            { "topic", new KeywordProperty() },
                            { "author", new TextProperty() },
                            {
                                "questions", new NestedProperty
                                {
                                    Properties = new Properties<Question>
                                    {
                                        { "title", new TextProperty { Analyzer = CustomAnalyzer } },
                                        { "description", new TextProperty { Analyzer = CustomAnalyzer } },
                                    }
                                } 
                                
                            },
                            {
                                "comments", new NestedProperty
                                {
                                    Properties = new Properties<Comment>
                                    {
                                        { "text", new TextProperty { Analyzer = CustomAnalyzer } }
                                    }
                                } 
                                
                            }
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