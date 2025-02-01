namespace Quiz.Persistence.Entities;

public class RefreshTokenEntity
{
    public string Id { get; set; }
    
    public string Token { get; set; }
    
    public DateTime Expires { get; set; }
    
    public bool IsUsed  { get; set; }
    
    public bool IsRevoked { get; set; }
    
    public string UserId { get; set; }
    
    public UserEntity UserEntity { get; set; }
}