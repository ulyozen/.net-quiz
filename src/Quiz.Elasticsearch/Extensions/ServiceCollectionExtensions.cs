using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quiz.Core.Abstractions;
using Quiz.Core.Repositories;
using Quiz.Elasticsearch.Common;
using Quiz.Elasticsearch.Repositories;

namespace Quiz.Elasticsearch.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddElasticsearch(this IServiceCollection services)
    {
        return services
            .AddSingleton<ISearchRepository, ElasticsearchRepository>()
            .AddHostedService<ElasticsearchInitializer>()
            .AddSingleton<ElasticsearchIndexManager>()
            .AddSingleton(ConnectElasticsearchClient);
    }
    
    private static ElasticsearchClient ConnectElasticsearchClient(this IServiceProvider provider)
    {
        var option = provider.GetService<IOptions<ElasticsearchOptions>>()!.Value;
                
        var connectionString = new ElasticsearchClientSettings(BuildUri(option))
            .Authentication(Auth(option));
                
        return new ElasticsearchClient(connectionString);
    }

    private static Uri BuildUri(ElasticsearchOptions env)
    {
        return new UriBuilder("http", env.ELASTICSEARCH_HOST, env.ELASTICSEARCH_PORT).Uri;
    }

    private static BasicAuthentication Auth(ElasticsearchOptions env)
    {
        return new BasicAuthentication(env.ELASTICSEARCH_USERNAME, env.ELASTICSEARCH_PASSWORD);
    }
}