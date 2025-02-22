namespace Quiz.Persistence.Entities;

public class QuestionTypeEntity : BaseEntity
{
    public int ValueType { get; set; }
    
    public List<QuestionEntity> Questions { get; set; }
}