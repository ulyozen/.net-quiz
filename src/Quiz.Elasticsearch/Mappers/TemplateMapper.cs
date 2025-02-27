using Quiz.Core.Entities;
using Quiz.Core.ValueObjects;
using Quiz.Elasticsearch.Entities;

namespace Quiz.Elasticsearch.Mappers;

public static class TemplateMapper
{
    public static TemplateIndex MapToIndex(this Template template)
    {
        return new TemplateIndex
        {
            TemplateId          = template.Id,
            TemplateTitle       = template.TemplateMetadata.Title,
            TemplateDescription = template.TemplateMetadata.Description,
            TemplateTopic       = template.TemplateMetadata.Topic,
            TemplateIsPublic    = template.TemplateMetadata.IsPublic,
            TemplateTags        = template.TemplateMetadata.Tags.ToHashSet(),
            QuestionTitle       = template.Questions.Select(q => q.Title ).ToList(),
            QuestionDescription = template.Questions
                .Where(q => q.Options != null)
                .SelectMany(q => q.Options!)
                .ToList()
        };
    }

    public static List<Template> MapToTemplates(this List<TemplateIndex> indices)
    {
        return indices.Select(MapToTemplate).ToList();
    }

    private static Template MapToTemplate(this TemplateIndex ti)
    {
        var tempMeta = TemplateMetadata.Create(
            ti.TemplateTitle,
            ti.TemplateDescription,
            ti.TemplateTopic,
            ti.TemplateIsPublic,
            ti.TemplateTags);
        
        return Template.Restore(ti.TemplateId, tempMeta);
    }
}