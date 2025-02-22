namespace Quiz.Application.Users.Dtos;

public class UsersResponse
{
    public string Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public string Role { get; set; }
    
    public bool IsBlocked { get; set; }
}