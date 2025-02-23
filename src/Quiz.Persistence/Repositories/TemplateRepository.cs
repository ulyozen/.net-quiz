using Microsoft.EntityFrameworkCore;
using Quiz.Application.Abstractions;
using Quiz.Core.Common;
using Quiz.Core.Entities;
using Quiz.Core.Repositories;
using Quiz.Persistence.Context;
using Quiz.Persistence.Entities;
using Quiz.Persistence.Mappers;

namespace Quiz.Persistence.Repositories;

public class TemplateRepository(AppDbContext context, IGuidFactory guidFactory) : ITemplateRepository
{
    public async Task<OperationResult<Template>> GetByIdAsync(string templateId)
    {
        var template = await context.Templates
            .Include(t => t.Comments)
            .Include(t => t.Likes)
            .Include(tt => tt.TemplateTags)
                .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(t => t.Id == templateId);
        
        return template is null 
            ? OperationResult<Template>.Failure("Template not found")
            : OperationResult<Template>.SuccessResult(template.MapToTemplate());
    }
    
    public async Task<IEnumerable<Template>> GetByUserIdAsync(string userId)
    {
        return (await context.Templates
            .Where(t => t.AuthorId == userId)
            .ToListAsync())
            .MapToTemplates();
    }
    
    public async Task<IEnumerable<Template>> GetPublicTemplatesAsync(int page, int pageSize, bool isPublic = true)
    {
        return (await context.Templates
            .Where(t => t.IsPublic == isPublic)
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync())
            .MapToTemplates();
    }
    
    public async Task<IEnumerable<Template>> GetPopularTemplatesAsync(int countTemp = 5)
    {
        return (await context.Templates
            .Select(t => new
            {
                Template = t,
                SubmissionCount = context.Submissions.Count(s => s.TemplateId == t.Id),
            })
            .OrderByDescending(t => t.SubmissionCount)
            .Take(countTemp)
            .Select(t => t.Template)
            .ToListAsync())
            .MapToTemplates();
    }
    
    public async Task<OperationResult> AddAsync(Template template)
    {
        var templateEntity = await AddTemplateAsync(template);
        
        await AddTemplateTagsAsync(templateEntity, template);
        
        await AddAllowUsersAsync(templateEntity, template);
        
        await context.SaveChangesAsync();
        
        return OperationResult.SuccessResult();
    }
    
    public async Task<OperationResult> UpdateAsync(Template template)
    {
        var existingTemplate = await context.Templates
            .Include(t => t.TemplateTags)
            .Include(t => t.AllowedUsers)
            .FirstOrDefaultAsync(t => t.Id == template.Id);
        
        if (existingTemplate is null) return OperationResult.Failure("Template not found");

        await ApplyTemplateUpdatesAsync(existingTemplate, template);
        
        context.Templates.Update(existingTemplate);
        
        return OperationResult.SuccessResult();
    }
    
    public async Task<OperationResult> DeleteAsync(string templateId)
    {
        var existingTemplate = await context.Templates.FindAsync(templateId);
        
        if (existingTemplate is null) return OperationResult.Failure("Template not found");
        
        context.Templates.Remove(existingTemplate);
        
        return OperationResult.SuccessResult();
    }
    
    private async Task<TemplateEntity> AddTemplateAsync(Template template)
    {
        var templateEntity = template.MapToEntity(guidFactory.Create());
        
        await context.Templates.AddAsync(templateEntity);
        
        return templateEntity;
    }
    
    private async Task AddTemplateTagsAsync(TemplateEntity templateEntity, Template template)
    {
        var tags = template.TemplateMetadata.Tags;
        
        var existingTags = await context.Tags
            .Where(t => tags.Contains(t.Name))
            .ToListAsync();

        await AddTagsAsync(tags, existingTags);
        
        var templateTags = existingTags.MapToTemplateTag(templateEntity);
        
        await context.TemplateTags.AddRangeAsync(templateTags);
    }
    
    private async Task AddTagsAsync(IReadOnlyList<string> tags, List<TagEntity> existingTags)
    {
        var newTagNames = tags.Except(existingTags.Select(t => t.Name)).ToList();
        
        if (newTagNames.Count == 0) return;
        
        var newTags = newTagNames.Select(tagName => new TagEntity
        {
            Id = guidFactory.Create(),
            Name = tagName
        }).ToList();
        
        await context.Tags.AddRangeAsync(newTags);
        
        existingTags.AddRange(newTags);
    }
    
    private async Task AddAllowUsersAsync(TemplateEntity templateEntity, Template template)
    {
        if (template.TemplateMetadata.IsPublic) return;
        
        var allowedUsers = new List<AllowedUsers>();
        
        allowedUsers.AddAllowUsers(templateEntity);
        
        allowedUsers.AddAllowUsers(templateEntity, template);
        
        await context.AllowedUsers.AddRangeAsync(allowedUsers);
    }

    private async Task ApplyTemplateUpdatesAsync(TemplateEntity templateEntity, Template template)
    {
        templateEntity.UpdateFieldsFrom(template);
        
        await UpdateTemplateTagsAsync(templateEntity, template);
        
        await UpdateAllowedUsersAsync(templateEntity, template);
    }
    
    private async Task UpdateTemplateTagsAsync(TemplateEntity templateEntity, Template template)
    {
        context.TemplateTags.RemoveRange(templateEntity.TemplateTags);
        
        await AddTemplateTagsAsync(templateEntity, template);
    }
    
    private async Task UpdateAllowedUsersAsync(TemplateEntity templateEntity, Template template)
    {
        context.AllowedUsers.RemoveRange(templateEntity.AllowedUsers);
        
        await AddAllowUsersAsync(templateEntity, template);
    }
}