using Quiz.Application.Abstractions;
using Quiz.Application.Templates.Commands;
using Quiz.Application.Templates.Dtos;
using Quiz.Core.Common;
using Quiz.Core.DomainEnums;
using Quiz.Core.Entities;
using Quiz.Core.ValueObjects;

namespace Quiz.Application.Mappers;

public static class TemplateMapper
{
    public static PaginationResult<TemplateDto> MapToDto(this PaginationResult<Template> templates)
    {
        var result = templates.Items.Select(t => t.MapToDto()).ToList();

        return PaginationResult<TemplateDto>.Create(
            result,
            templates.TotalCount,
            templates.Page,
            templates.PageSize);
    }
    
    public static List<TemplateDto> MapToDto(this IList<Template> templates)
    {
        return templates.Select(MapToDto).ToList();
    }
    
    public static Template MapToTemplate(this CreateTemplateCommand command, IGuidFactory guidFactory)
    {
        var templateMetadata = command.MapToTemplateMetadata();

        var question         = command.MapToQuestionsDomain(guidFactory);
        
        return Template.Create(
            guidFactory.Create(),
            templateMetadata,
            command.AuthorId,
            command.AuthorName,
            command.ImageUrl,
            command.CreatedAt,
            question);
    }

    private static List<Question> MapToQuestionsDomain(this CreateTemplateCommand command, IGuidFactory guidFactory)
    {
        return command.Questions.Select(q =>
        {
            var title   = q.Title;
            var type    = ParseQuestionType(q.Type);
            var options = q.Options;
            var answer  = q.CorrectAnswers;

            if (!AreAnswersValid(type, options, answer))
                throw new ArgumentException($"Invalid answers for question type: {type}");
            
            return Question.Create(guidFactory.Create(), title, type, options, answer);
        }).ToList();
    }
    
    private static TemplateDto MapToDto(this Template template)
    {
        return new TemplateDto
        {
            TemplateId  = template.Id,
            Title       = template.TemplateMetadata.Title,
            Description = template.TemplateMetadata.Description,
            Topic       = template.TemplateMetadata.Topic,
            IsPublic    = template.TemplateMetadata.IsPublic,
            Tags        = template.TemplateMetadata.Tags.ToHashSet()
        };
    }
    
    private static TemplateMetadata MapToTemplateMetadata(this CreateTemplateCommand command)
    {
        return TemplateMetadata.Create(
            command.Title,
            command.Description,
            command.Topic,
            command.IsPublic,
            command.Tags,
            command.AllowedUsers);
    }
    
    private static QuestionType ParseQuestionType(string type)
    {
        return type switch
        {
            "SingleChoice"   => QuestionType.SingleChoice,
            "MultipleChoice" => QuestionType.MultipleChoice,
            "OpenText"       => QuestionType.OpenText,
            "Boolean"        => QuestionType.Boolean,
            _                => ThrowUnexpectedQuestionType(type)
        };
    }
    
    private static QuestionType ThrowUnexpectedQuestionType(string type)
    {
        throw new ArgumentOutOfRangeException(nameof(type), $"Unexpected question type: {type}");
    }
    
    private static bool AreAnswersValid(QuestionType type, List<string>? options, List<string>? correctAnswers)
    {
        if (type is QuestionType.OpenText) return true;
        
        if (options == null || correctAnswers == null) return false;
        
        return correctAnswers.All(answer => options.Contains(answer));
    }
}