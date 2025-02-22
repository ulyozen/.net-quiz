using Quiz.Core.Entities;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Mappers;

public static class AllowUsersMapper
{
    public static void AddAllowUsers(this List<AllowedUsers> allowUsers, TemplateEntity templateEntity)
    {
        allowUsers.Add(new AllowedUsers { UserId = templateEntity.AuthorId, TemplateId = templateEntity.Id });
    }
    
    public static void AddAllowUsers(this List<AllowedUsers> allowUsers, TemplateEntity templateEntity, Template template)
    {
        if (template.TemplateMetadata.AllowedUsers is not null)
        {
            allowUsers.AddRange(template.TemplateMetadata.AllowedUsers.Select(userId => new AllowedUsers
            {
                UserId = userId,
                TemplateId = templateEntity.Id,
            }));
        }
    }
}