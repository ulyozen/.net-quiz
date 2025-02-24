using System.Text.Json.Serialization;
using Quiz.Core.DomainEnums;

namespace Quiz.Core.Entities;

public class Question : BaseEntity
{
    public string Title { get; private set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public QuestionType QuestionType { get; private set; }
    
    public List<string>? Options { get; set; }
    
    public List<string>? CorrectAnswers { get; set; }
    
    private Question(string id, string title, QuestionType questionType, 
        List<string>? options, List<string>? correctAnswers)
    {
        Id = id;
        Title = title;
        QuestionType = questionType;
        Options = options;
        CorrectAnswers = correctAnswers;
    }

    private Question(string questionId, string title, QuestionType questionType, List<string>? options)
    {
        Id = questionId;
        Title = title;
        QuestionType = questionType;
        Options = options;
    }
    
    public static Question Create(string id, string title, QuestionType questionType, 
        List<string>? options, List<string>? correctAnswers)
    {
        return new Question(id, title, questionType, options, correctAnswers);
    }
    
    public static Question Restore(string questionId, string title, QuestionType questionType, List<string>? options)
    {
        return new Question(questionId, title, questionType, options);
    }
}