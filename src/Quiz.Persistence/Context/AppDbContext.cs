using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quiz.Persistence.Configurations;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Context;

public class AppDbContext : IdentityDbContext<UserEntity>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<AllowedUsers> AllowedUsers { get; set; }
    
    public DbSet<CommentEntity> Comments { get; set; }
    
    public DbSet<LikeEntity> Likes { get; set; }
    
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    
    public DbSet<SubmissionEntity> Submissions { get; set; }
    
    public DbSet<TagEntity> Tags { get; set; }
    
    public DbSet<TemplateEntity> Templates { get; set; }
    
    public DbSet<TemplateTag> TemplateTags { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}