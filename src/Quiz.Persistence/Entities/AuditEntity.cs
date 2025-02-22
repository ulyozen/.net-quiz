namespace Quiz.Persistence.Entities;

public class AuditEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}