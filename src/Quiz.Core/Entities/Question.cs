using Quiz.Core.DomainEnums;

namespace Quiz.Core.Entities;

public class Question : BaseEntity
{
    public string Title { get; private set; }
    
    public QuestionType QuestionType { get; private set; }
    
    private Question() { }
    
    private Question(string title, QuestionType questionType)
    {
        Title = title;
        QuestionType = questionType;
    }
    
    public static Question Create(string title, QuestionType questionType)
    {
        return new Question(title, questionType);
    }
    
    public static Question Restore(string questionId, string title, QuestionType questionType)
    {
        return new Question
        {
            Id = questionId,
            Title = title,
            QuestionType = questionType
        };
    }
}