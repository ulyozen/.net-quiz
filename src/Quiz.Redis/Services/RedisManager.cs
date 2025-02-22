using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Quiz.Application.Abstractions;
using Quiz.Core.Common;
using StackExchange.Redis;

namespace Quiz.Redis.Services;

public class RedisManager : ICacheManager
{
    private readonly ILogger<RedisManager> _logger;
    
    private readonly IDatabase _redis;
    
    private const int CacheDuration = 30;
    
    public RedisManager(ILogger<RedisManager> logger, IConnectionMultiplexer redis)
    {
        _logger = logger;
        _redis = redis.GetDatabase();
    }
    
    public async Task SetAsync<T>(string key, T value, int? expiry = null)
    {
        try
        {
            var json = JsonSerializer.Serialize(value);

            await _redis.StringSetAsync(key, json, ExpiryAfter(expiry));
            
            _logger.LogInformation(
                "Saving to Redis: Key={Key}, Value={Value}, ActualType={Type}", 
                key, json, GetReadableTypeName(typeof(T)));
        }
        catch (JsonException err)
        {
            _logger.LogError(
                err, 
                "Failed to serialize value before saving to Redis: Key={Key}, ActualType={Type}", 
                key, GetReadableTypeName(typeof(T)));
        }
        catch (RedisConnectionException err)
        {
            _logger.LogError(
                err, 
                "Redis connection error while writing: Key={Key}, ActualType={Type}", 
                key, GetReadableTypeName(typeof(T)));
        }
        catch (Exception err)
        {
            _logger.LogError(err, "Failed to write to Redis: Key={Key}", key);
        }
    }
    
    public async Task<OperationResult<T>> GetAsync<T>(string key)
    {
        var json = RedisValue.Null;
        
        try
        {
            json = await _redis.StringGetAsync(key);
            
            if (!json.IsNullOrEmpty) 
                return OperationResult<T>.SuccessResult(JsonSerializer.Deserialize<T>(json!));
            
            _logger.LogWarning("Redis key not found: {Key}", key);
            
            return OperationResult<T>.Failure($"Redis key not found: {key}");
        }
        catch (JsonException  err)
        {
            _logger.LogError(
                err, 
                "Failed to deserialize Redis value: Key={Key}, Value={Value}, ActualType = {ActualType}, ExpectedType={ExpectedType}", 
                key, json, GetReadableTypeNameFromJson(json), GetReadableTypeName(typeof(T)));
            
            return OperationResult<T>.Failure("Failed to deserialize Redis value");
        }
        catch (RedisTimeoutException err)
        {
            _logger.LogWarning(err, "Redis timeout while reading key={Key}", key);
            
            return OperationResult<T>.Failure("Redis timeout occurred");
        }
        catch (RedisConnectionException err)
        {
            _logger.LogError(err, "Redis connection error while reading Key={Key}", key);
            
            return OperationResult<T>.Failure("Redis connection error");
        }
        catch (Exception err)
        {
            _logger.LogError(err, "Error reading from Redis: Key={Key}", key);
            
            return OperationResult<T>.Failure($"Cache error: {err.Message}");
        }
    }
    
    public async Task RemoveAsync(string key)
    {
        try
        {
            await _redis.KeyDeleteAsync(key);
        }
        catch (Exception err)
        {
            _logger.LogError(err, "Error deleting from Redis: Key={Key}", key);
        }
    }
    
    private static string GetReadableTypeName(Type type)
    {
        if (!type.IsGenericType) return type.Name;
        
        var genericArgs = string.Join(", ", type.GetGenericArguments().Select(GetReadableTypeName));
        
        return $"{type.Name.Split('`')[0]}<{genericArgs}>";
    }
    
    private static string GetReadableTypeNameFromJson(RedisValue json)
    {
        if (json.IsNullOrEmpty) return "Null";
        
        var type = JsonSerializer.Deserialize<JsonElement>(json.ToString());
        
        if (type.ValueKind is JsonValueKind.Array ) return "Array";
        if (type.ValueKind is JsonValueKind.Object ) return "Object";
        if (type.ValueKind is JsonValueKind.String ) return "String";
        if (type.ValueKind is JsonValueKind.Number ) return "Number";
        if (type.ValueKind == JsonValueKind.True || 
            type.ValueKind == JsonValueKind.False) 
            return "Boolean";
        
        return "Unknown";
    }
    
    private static TimeSpan ExpiryAfter(int? time) => TimeSpan.FromMinutes(time ?? CacheDuration);
}