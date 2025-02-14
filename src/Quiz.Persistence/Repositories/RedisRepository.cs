using System.Text.Json;
using Quiz.Application.Abstractions;
using StackExchange.Redis;

namespace Quiz.Persistence.Repositories;

public class RedisRepository(IConnectionMultiplexer redis) : IRedisRepository
{
    private readonly IDatabase _redis = redis.GetDatabase();
    
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);
        await _redis.StringSetAsync(key, json, expiry ?? TimeSpan.FromMinutes(10));
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var json = await _redis.StringGetAsync(key);
        return json.IsNullOrEmpty ? default : JsonSerializer.Deserialize<T>(json!);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await _redis.KeyExistsAsync(key);
    }

    public async Task RemoveAsync(string key)
    {
        await _redis.KeyDeleteAsync(key);
    }
}