using Quiz.Core.Abstractions;
using Quiz.Core.DomainEvents;

namespace Quiz.Core.Entities;

public class Submission : BaseEntity, IHasDomainEvent
{
    private readonly List<IDomainEvent> _domainEvents = new();

    private readonly List<IAnswer> _answers;
    
    public string TemplateId { get; private set; }
    
    public string UserId { get; private set; }
    
    public IReadOnlyList<IAnswer> Answers => _answers;

    private Submission(List<IAnswer> answers)
    {
        _answers = answers;
    }
    
    private Submission(string templateId, string userId, List<IAnswer> answers)
    {
        TemplateId = templateId;
        UserId = userId;
        _answers = answers;
        
        _domainEvents.Add(SubmissionEvent.Create(templateId, userId));
    }
    
    public void AddAnswer<T>(string questionId, T answerText)
    {
        _answers.Add(Answer<T>.Create(questionId, answerText));
    }
    
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents;
    
    public void ClearDomainEvents() => _domainEvents.Clear();
    
    public static Submission Create(string templateId, string userId, List<IAnswer> answers)
    {
        return new Submission(templateId, userId, answers);
    }

    public static Submission Restore(string templateId, string userId, List<IAnswer> answers)
    {
        return new Submission(answers)
        {
            TemplateId = templateId,
            UserId = userId
        };
    }
}