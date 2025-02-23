using Quiz.Application.Templates.Commands;
using Quiz.Application.Templates.Dtos;
using Quiz.Core.Entities;
using Quiz.Core.ValueObjects;

namespace Quiz.Application.Mappers;

public static class TemplateMapper
{
    public static IEnumerable<PopularTemplate> MapToPopularTemplate(this IEnumerable<Template> templates)
    {
        return templates.Select(MapToPopularTemplate);
    }
    
    public static Template MapToTemplate(this CreateTemplateCommand command)
    {
        var templateMetadata = command.MapToTemplateMetadata();
        
        return Template.Create(
            templateMetadata,
            command.AuthorId,
            command.AuthorName,
            command.ImageUrl,
            command.CreatedAt);
    }
    
    private static PopularTemplate MapToPopularTemplate(this Template template)
    {
        return new PopularTemplate
        {
            TemplateId = template.Id,
            Title = template.TemplateMetadata.Title,
            Description = template.TemplateMetadata.Description,
            Topic = template.TemplateMetadata.Topic,
            IsPublic = template.TemplateMetadata.IsPublic,
            Tags = template.TemplateMetadata.Tags
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
}