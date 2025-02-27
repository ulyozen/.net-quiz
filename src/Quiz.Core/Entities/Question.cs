using System.Text.Json.Serialization;
using Quiz.Core.DomainEnums;

namespace Quiz.Core.Entities;

public class Question : Entity
{
    public string Title { get; private set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public QuestionType QuestionType { get; private set; }
    
    public List<string>? Options { get; private set; }
    
    public List<string>? CorrectAnswers { get; private set; }
    
    private Question(string id, string title, QuestionType questionType, 
        List<string>? options, List<string>? correctAnswers) : base(id)
    {
        Title          = title;
        QuestionType   = questionType;
        Options        = options;
        CorrectAnswers = correctAnswers;
    }
    
    private Question(string id, string title, QuestionType questionType, 
        List<string>? options) : base(id)
    {
        Title        = title;
        QuestionType = questionType;
        Options      = options;
    }
    
    public static Question Create(string id, string title, QuestionType questionType, 
        List<string>? options, List<string>? correctAnswers)
    {
        return new Question(id, title, questionType, options, correctAnswers);
    }
    
    public static Question Restore(string id, string title, QuestionType questionType, 
        List<string>? options)
    {
        return new Question(id, title, questionType, options);
    }
}