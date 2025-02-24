using System.Text.Json;
using Quiz.Core.DomainEnums;
using Quiz.Core.Entities;
using Quiz.Core.ValueObjects;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Mappers;

public static class TemplateMapper
{
    public static List<Template> MapToTemplates(this IEnumerable<TemplateEntity> entities)
    {
        return entities.Select(MapToTemplate).ToList();
    }
    
    public static Template MapToTemplate(this TemplateEntity entity)
    {
        var tempMeta = entity.MapToTemplateMetadata();
        
        var questions = entity.Questions.Select(que => 
            Question.Restore(
                que.Id, 
                que.Title, 
                que.QuestionType,
                que.Options is null 
                    ? null 
                    : JsonSerializer.Deserialize<List<string>>(que.Options))).ToList();
        
        return Template.Restore(
            entity.Id, 
            tempMeta, 
            entity.AuthorId, 
            entity.AuthorName, 
            entity.ImageUrl, 
            entity.CreatedAt, 
            entity.UpdatedAt,
            questions);
    }
    
    public static List<Template> MapToPopular(this IEnumerable<TemplateEntity> entities)
    {
        return entities.Select(MapToPopular).ToList();
    }
    
    public static void UpdateFieldsFrom(this TemplateEntity entity, Template template)
    {
        entity.Title       = template.TemplateMetadata.Title;
        entity.Description = template.TemplateMetadata.Description;
        entity.Topic       = template.TemplateMetadata.Topic;
        entity.IsPublic    = template.TemplateMetadata.IsPublic;
        entity.ImageUrl    = template.ImageUrl;
    }
    
    public static TemplateEntity MapToEntity(this Template template, string templateId)
    {
        var questions = template.Questions.Select(question => new QuestionEntity
        {
            Id             = question.Id,
            TemplateId     = templateId,
            Title          = question.Title,
            QuestionType   = question.QuestionType,
            Options        = question.Options is null ? null : JsonSerializer.Serialize(question.Options),
            CorrectAnswers = question.CorrectAnswers is null ? null : JsonSerializer.Serialize(question.CorrectAnswers)
        }).ToList();
        
        return new TemplateEntity
        {
            Id          = templateId,
            AuthorId    = template.AuthorId,
            AuthorName  = template.AuthorName,
            Title       = template.TemplateMetadata.Title,
            Description = template.TemplateMetadata.Description,
            Topic       = template.TemplateMetadata.Topic,
            IsPublic    = template.TemplateMetadata.IsPublic,
            ImageUrl    = template.ImageUrl,
            CreatedAt   = template.CreatedAt,
            UpdatedAt   = template.UpdatedAt,
            Questions   = questions
        };
    }
    
    public static IEnumerable<TemplateTag> MapToTemplateTag(this List<TagEntity> tags, TemplateEntity templateEntity)
    {
        return tags.Select(t => new TemplateTag
        {
            TemplateId = templateEntity.Id,
            TagId      = t.Id,
        });
    }
    
    private static Template MapToPopular(this TemplateEntity entity)
    {
        return Template.Restore(entity.Id, entity.MapToPopularTempMeta());
    }
    
    private static TemplateMetadata MapToPopularTempMeta(this TemplateEntity entity)
    {
        return TemplateMetadata.Create(
            entity.Title,
            entity.Description,
            entity.Topic,
            entity.IsPublic,
            entity.TemplateTags.Select(tt => tt.Tag.Name).ToList());
    }
    
    private static TemplateMetadata MapToTemplateMetadata(this TemplateEntity entity)
    {
        return TemplateMetadata.Create(
            entity.Title,
            entity.Description,
            entity.Topic,
            entity.IsPublic,
            entity.TemplateTags.Select(tt => tt.Tag.Name).ToList(),
            entity.IsPublic
                ? []
                : entity.AllowedUsers.Select(au => au.UserId).ToList());
    }
}