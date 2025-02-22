using Quiz.Core.Common;

namespace Quiz.Application.Abstractions;

public interface ICacheManager
{
    Task SetAsync<T>(string key, T value, int? expiry = null);
    
    Task<OperationResult<T>> GetAsync<T>(string key);
    
    Task RemoveAsync(string key);
}