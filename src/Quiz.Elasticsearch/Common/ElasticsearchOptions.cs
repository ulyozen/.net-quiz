using System.ComponentModel.DataAnnotations;

namespace Quiz.Elasticsearch.Common;

public class ElasticsearchOptions
{
    [Required]
    public string ELASTICSEARCH_USERNAME { get; set; }
    
    [Required]
    public string ELASTICSEARCH_PASSWORD { get; set; }
    
    [Required]
    public string ELASTICSEARCH_HOST { get; set; }
    
    [Range(1, 65535)]
    public int ELASTICSEARCH_PORT { get; set; }
}