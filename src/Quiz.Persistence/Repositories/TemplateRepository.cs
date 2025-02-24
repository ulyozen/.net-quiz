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
            .Include(t => t.Questions)
            .Include(t => t.Comments)
            .Include(t => t.Likes)
            .Include(tt => tt.TemplateTags)
                .ThenInclude(t => t.Tag)
            .FirstOrDefaultAsync(t => t.Id == templateId);
        
        return template is null
            ? OperationResult<Template>.Failure(DomainErrors.Template.TemplateNotFound)
            : OperationResult<Template>.SuccessResult(template.MapToTemplate());
    }
    
    public async Task<PaginationResult<Template>> GetTemplatesAsync(int page, int pageSize)
    {
        var totalCount = await context.Templates.CountAsync();
        
        var templates = await context.Templates
            .Include(t => t.TemplateTags)
                .ThenInclude(t => t.Tag)
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var asd = templates.MapToIntro();
        
        return PaginationResult<Template>.Create(asd, totalCount, page, pageSize);
    }
    
    public async Task<IEnumerable<Template>> GetPopularTemplatesAsync(int totalTemp = 5)
    {
        var templates = await context.Templates
            .Include(t => t.TemplateTags)
                .ThenInclude(t => t.Tag)
            .GroupJoin(
                context.Submissions,
                template => template.Id,
                submission => submission.TemplateId,
                (template, submission) => new
                {
                    Template = template, 
                    SubmissionCount = submission.Count()
                })
            .OrderByDescending(t => t.SubmissionCount)
            .Take(totalTemp)
            .Select(t => t.Template)
            .ToListAsync();
        
        return templates.MapToIntro();
    }
    
    public async Task<OperationResult> AddAsync(Template template)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        
        try
        {
            var templateEntity = await AddTemplateAsync(template);
            
            await AddQuestionsAsync(templateEntity);
            
            await AddTemplateTagsAsync(templateEntity, template);
            
            await AddAllowUsersAsync(templateEntity, template);
            
            await context.SaveChangesAsync();
            
            await transaction.CommitAsync();
            
            return OperationResult.SuccessResult();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            
            return OperationResult.Failure(e);
        }
    }
    
    public async Task<OperationResult> UpdateAsync(Template template)
    {
        var existingTemplate = await context.Templates
            .Include(t => t.TemplateTags)
            .Include(t => t.AllowedUsers)
            .FirstOrDefaultAsync(t => t.Id == template.Id);
        
        if (existingTemplate is null) 
            return OperationResult.Failure(DomainErrors.Template.TemplateNotFound);

        await ApplyTemplateUpdatesAsync(existingTemplate, template);
        
        context.Templates.Update(existingTemplate);
        
        return OperationResult.SuccessResult();
    }
    
    public async Task<OperationResult> DeleteAsync(string templateId)
    {
        try
        {
            var existingTemplate = await context.Templates.FindAsync(templateId);
            
            if (existingTemplate is null) 
                return OperationResult.Failure(DomainErrors.Template.TemplateNotFound);
            
            context.Templates.Remove(existingTemplate);
            
            await context.SaveChangesAsync();
            
            return OperationResult.SuccessResult();
        }
        catch (Exception e)
        {
            return OperationResult.Failure(e);
        }
    }
    
    private async Task<TemplateEntity> AddTemplateAsync(Template template)
    {
        var templateEntity = template.MapToEntity(guidFactory.Create());
        
        await context.Templates.AddAsync(templateEntity);
        
        return templateEntity;
    }
    
    private async Task AddQuestionsAsync(TemplateEntity templateEntity)
    {
        await context.Questions.AddRangeAsync(templateEntity.Questions);
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