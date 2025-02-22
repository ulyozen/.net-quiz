using Microsoft.Extensions.Options;
using Quiz.Api.Configuration;
using Quiz.Application.Common;
using Quiz.Elasticsearch.Common;
using Quiz.Persistence.Common;
using Quiz.Redis.Common;
using Quiz.Serilog.Common;

namespace Quiz.Api.Extensions;

public static class EnvironmentExtension
{
    public static IServiceCollection AddEnvironmentVariables(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .ConfigureWithValidation<PostgreSqlOptions>(configuration)
            .ConfigureWithValidation<JwtOptions>(configuration)
            .ConfigureWithValidation<RedisOptions>(configuration)
            .ConfigureWithValidation<ElasticsearchOptions>(configuration)
            .ConfigureWithValidation<SerilogOptions>(configuration);
    }
    
    private static IServiceCollection ConfigureWithValidation<T>(this IServiceCollection services, IConfiguration configuration) where T : class, new()
    {
        return services
            .Configure<T>(configuration)
            .AddSingleton<IValidateOptions<T>, ValidateConfig<T>>();
    }
}