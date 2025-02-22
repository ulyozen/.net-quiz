using System.ComponentModel.DataAnnotations;

namespace Quiz.Serilog.Common;

public class SerilogOptions
{
    [Required]
    public string ELASTICSEARCH_HOST { get; set; }
    
    [Range(1, 65535)]
    public int ELASTICSEARCH_PORT { get; set; }
}