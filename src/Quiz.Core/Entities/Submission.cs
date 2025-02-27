using Quiz.Core.Abstractions;
using Quiz.Core.DomainEvents;

namespace Quiz.Core.Entities;

public class Submission : AggregateRoot
{
    private readonly List<IAnswer> _answers = new();
    
    public string TemplateId { get; private set; }
    
    public string UserId { get; private set; }
    
    public IReadOnlyList<IAnswer> Answers => _answers;
    
    private Submission(string id,  List<IAnswer> answers) : base(id)
    {
        _answers.AddRange(answers);
    }
    
    private Submission(string id, string templateId, string userId, List<IAnswer> answers) : base(id)
    {
        TemplateId = templateId;
        UserId     = userId;
        _answers   = answers;
        
        RaiseDomainEvent(SubmissionEvent.Create(templateId, userId));
    }
    
    public void AddAnswer<T>(string questionId, T answerText)
    {
        _answers.Add(Answer<T>.Create(questionId, answerText));
    }
    
    public static Submission Create(string id, string templateId, string userId, List<IAnswer> answers)
    {
        return new Submission(id, templateId, userId, answers);
    }
    
    public static Submission Restore(string id, string templateId, string userId, List<IAnswer> answers)
    {
        return new Submission(id, answers)
        {
            TemplateId = templateId,
            UserId     = userId
        };
    }
}