using System.ComponentModel.DataAnnotations;

namespace Quiz.Redis.Common;

public class RedisOptions
{
    [Required]
    public string REDIS_HOST { get; set; }
    
    [Range(1, 65535)]
    public int REDIS_PORT { get; set; }
    
    [Required]
    public string? REDIS_PASSWORD { get; set; }
}