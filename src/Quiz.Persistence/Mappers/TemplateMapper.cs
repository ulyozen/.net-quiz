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
        return Template.Restore(
            entity.Id, 
            entity.MapToTemplateMetadata(), 
            entity.AuthorId, 
            entity.AuthorName, 
            entity.ImageUrl, 
            entity.CreatedAt, 
            entity.UpdatedAt);
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
        return new TemplateEntity
        {
            Id          = templateId,
            AuthorId    = template.AuthorId,
            AuthorName  = template.AuthorName,
            Title       = template.TemplateMetadata.Title,
            Description = template.TemplateMetadata.Description,
            Topic       = template.TemplateMetadata.Topic,
            ImageUrl    = template.ImageUrl,
            CreatedAt   = template.CreatedAt,
            UpdatedAt   = template.UpdatedAt
        };
    }
    
    public static IEnumerable<TemplateTagEntity> MapToTemplateTag(this List<TagEntity> tags, TemplateEntity templateEntity)
    {
        return tags.Select(t => new TemplateTagEntity
        {
            TemplateId = templateEntity.Id,
            TagId = t.Id,
        });
    }
    
    private static TemplateMetadata MapToTemplateMetadata(this TemplateEntity entity)
    {
        return TemplateMetadata.Create(
            entity.Title,
            entity.Description,
            entity.Topic,
            entity.IsPublic,
            entity.TemplateTags.Select(tt => tt.Tag.Name).ToList(),
            entity.AllowedUsers.Select(au => au.UserId).ToList());
    }
}