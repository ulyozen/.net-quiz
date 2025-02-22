using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quiz.Application.Abstractions;
using Quiz.Persistence.Common;
using Quiz.Redis.Common;
using Quiz.Redis.Services;
using StackExchange.Redis;

namespace Quiz.Redis.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddRedis(this IServiceCollection services)
    {
        services.AddScoped<ICacheManager, RedisManager>();
        services.AddScoped<IUserCache, UserCache>();
        
        services.AddSingleton<IConnectionMultiplexer>(options =>
        {
            var redisOptions = options.GetRequiredService<IOptions<RedisOptions>>().Value;

            return ConnectionMultiplexer.Connect(BuildConnectionString(redisOptions));
        });
        
        services.AddSingleton<IDatabase>(options => 
            options.GetRequiredService<IConnectionMultiplexer>().GetDatabase());
    }
    
    private static string BuildConnectionString(RedisOptions env)
    {
        return $"{env.REDIS_HOST}:{env.REDIS_PORT},password={env.REDIS_PASSWORD}";
    }
}