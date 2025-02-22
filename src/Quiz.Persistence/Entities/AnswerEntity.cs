namespace Quiz.Persistence.Entities;

public class AnswerEntity : BaseEntity
{
    public string? AnswerText { get; set; }
    
    public int? AnswerValue { get; set; }
    
    public bool? AnswerBoolean { get; set; }
    
    public string QuestionId { get; set; }
    
    public QuestionEntity Question { get; set; }
    
    public string SubmissionId { get; set; }
    
    public SubmissionEntity Submission { get; set; }
}

