namespace Quiz.Persistence.Common;

public class DatabaseConnectionOptions
{
    public string? Database { get; set; }
    
    public string? User { get; set; }
    
    public string? Password { get; set; }
    
    public string? Host { get; set; }
    
    public int? Port { get; set; }
}