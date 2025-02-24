using System.Text.Json.Serialization;
using Quiz.Core.DomainEnums;

namespace Quiz.Persistence.Entities;

public class QuestionEntity : BaseEntity
{
    public string Title { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public QuestionType QuestionType { get; set; }
    
    public string? Options { get; set; }
    
    public string? CorrectAnswers { get; set; }
    
    public string TemplateId { get; set; }
    
    public TemplateEntity Template { get; set; }
}