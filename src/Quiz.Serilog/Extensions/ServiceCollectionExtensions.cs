using System.Threading.Channels;
using Elastic.Apm.SerilogEnricher;
using Elastic.Channels;
using Elastic.CommonSchema;
using Elastic.CommonSchema.Serilog;
using Elastic.Ingest.Elasticsearch;
using Elastic.Ingest.Elasticsearch.DataStreams;
using Elastic.Serilog.Enrichers.Web;
using Elastic.Serilog.Sinks;
using Elastic.Transport;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quiz.Elasticsearch.Common;
using Quiz.Serilog.Common;
using Serilog;
using Serilog.Events;

namespace Quiz.Serilog.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddSerilogSupport(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((_, services, loggerConfig) =>
        {
            var serilogOptions = services.GetRequiredService<IOptions<SerilogOptions>>();
            var elasticOptions = services.GetRequiredService<IOptions<ElasticsearchOptions>>();
            var httpAccessor   = services.GetRequiredService<IHttpContextAccessor>();
            
            var host = serilogOptions.Value.ELASTICSEARCH_HOST;
            var port = serilogOptions.Value.ELASTICSEARCH_PORT;
            
            var baseUrl = new UriBuilder("http", host, port, "").Uri;
        
            var username = elasticOptions.Value.ELASTICSEARCH_USERNAME;
            var password = elasticOptions.Value.ELASTICSEARCH_PASSWORD;

            loggerConfig
                .MinimumLevel.Warning()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Quiz.Redis", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithEcsHttpContext(httpAccessor)
                .Enrich.WithElasticApmCorrelationInfo()
                .WriteTo.Console(new EcsTextFormatter())
                .WriteTo.Elasticsearch(
                    [baseUrl], 
                    options => options.ElasticsearchConfigure(), 
                    transport => transport.Authentication(new BasicAuthentication(username, password)));
        });
    }
    
    private static void ElasticsearchConfigure(this ElasticsearchSinkOptions options)
    {
        options.DataStream       = new DataStreamName("logs", "quiz", "net");
        options.BootstrapMethod  = BootstrapMethod.Failure;
        options.TextFormatting   = new EcsTextFormatterConfiguration<EcsDocument>();
        options.ConfigureChannel = channelOpts =>
        {
            channelOpts.BufferOptions = new BufferOptions
            {
                ExportMaxConcurrency = 1,
                ExportMaxRetries = 10,
                ExportBackoffPeriod = retry => TimeSpan.FromSeconds(Math.Pow(3, retry)),
                InboundBufferMaxSize = 20000,
                BoundedChannelFullMode = BoundedChannelFullMode.DropOldest,
                OutboundBufferMaxLifetime = TimeSpan.FromSeconds(5),
                OutboundBufferMaxSize = 100,
            };
        };
    }
}