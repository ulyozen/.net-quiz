using Quiz.Core.Abstractions;

namespace Quiz.Core.Entities;

public class Answer<T> : IAnswer
{
    public string QuestionId { get; private set; }
    
    public T Value { get; private set; }
    
    private Answer(string questionId, T value)
    {
        QuestionId = questionId;
        Value      = value;
    }
    
    public static Answer<T> Create(string questionId, T value) => new(questionId, value);
}