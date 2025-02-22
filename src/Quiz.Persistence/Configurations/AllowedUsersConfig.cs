using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Configurations;

public class AllowedUsersConfig : IEntityTypeConfiguration<AllowedUsers>
{
    public void Configure(EntityTypeBuilder<AllowedUsers> builder)
    {
        builder.ToTable("AllowedUsers");
        
        builder.HasKey(au => new { au.UserId, au.TemplateId })
            .HasName("PK_Composite_AllowedUsers");
        
        builder.HasOne(au => au.User)
            .WithMany(u => u.AllowedTemplates)
            .HasForeignKey(au => au.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(au => au.Template)
            .WithMany(t => t.AllowedUsers)
            .HasForeignKey(au => au.TemplateId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(au => au.TemplateId)
            .HasDatabaseName("IX_TemplateId_AllowedUsers");
    }
}