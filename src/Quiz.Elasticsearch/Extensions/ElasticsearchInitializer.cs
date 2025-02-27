using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quiz.Elasticsearch.Common;
using Quiz.Elasticsearch.Config;

namespace Quiz.Elasticsearch.Extensions;

public class ElasticsearchInitializer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    
    private readonly ILogger<ElasticsearchInitializer> _logger;
    
    private const int RetryIntervalMinutes = 2;
    
    public ElasticsearchInitializer(
        IServiceScopeFactory scopeFactory, 
        ILogger<ElasticsearchInitializer> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var indexManager = scope.ServiceProvider.GetService<IndexManager>();
            
            if (indexManager is null)
            {
                _logger.LogError("ElasticsearchIndexManager not found in DI container.");
                return;
            }
            
            try
            {
                await indexManager.CreateIndexAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Elasticsearch initialization failed.");
            }
            
            await Task.Delay(TimeSpan.FromMinutes(RetryIntervalMinutes), stoppingToken);
        }
        
    }
}