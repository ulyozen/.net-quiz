namespace Quiz.Persistence.Entities;

public class FullAuditEntity : AuditEntity
{
    public string? CreatedBy { get; set; }
    
    public string? UpdatedBy { get; set; }
}