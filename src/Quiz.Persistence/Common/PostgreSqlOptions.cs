using System.ComponentModel.DataAnnotations;

namespace Quiz.Persistence.Common;

public class PostgreSqlOptions
{
    [Required]
    public string POSTGRES_USER { get; set; }
    
    [Required]
    public string POSTGRES_PASSWORD { get; set; }
    
    [Required]
    public string POSTGRES_DB { get; set; }
    
    [Required]
    public string POSTGRES_HOST { get; set; }
    
    [Range(1, 65535)]
    public int POSTGRES_PORT { get; set; }
}