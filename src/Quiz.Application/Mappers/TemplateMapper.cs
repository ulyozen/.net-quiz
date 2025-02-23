using Quiz.Application.Templates.Commands;
using Quiz.Core.Entities;
using Quiz.Core.ValueObjects;

namespace Quiz.Application.Mappers;

public static class TemplateMapper
{
    public static Template MapToTemplate(this CreateTemplateCommand command)
    {
             var templateMetadata = command.MapToTemplateMetadata();
             
             return Template.Create(
                 templateMetadata,
                 command.AuthorId,
                 command.AuthorName,
                 command.ImageUrl,
                 command.CreatedAt
                 );
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