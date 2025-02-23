using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Core.Entities;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Configurations;

public class TemplateConfig : IEntityTypeConfiguration<TemplateEntity>
{
    public void Configure(EntityTypeBuilder<TemplateEntity> builder)
    {
        builder.ToTable("Templates");
        
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.AuthorId)
            .IsRequired();

        builder.Property(t => t.AuthorName)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(t => t.Title)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(t => t.Description)
            .HasColumnType("text")
            .IsRequired();
        
        builder.Property(t => t.Topic)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(t => t.IsPublic)
            .HasDefaultValue(true)
            .IsRequired();
        
        builder.Property(t => t.ImageUrl)
            .HasColumnType("text")
            .IsRequired(false);
        
        builder.Property(t => t.CreatedAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired();
        
        builder.Property(t => t.UpdatedAt)
            .HasColumnType("timestamp with time zone")
            .IsRequired(false);
        
        builder.Property(t => t.CreatedBy)
            .HasColumnType("text")
            .IsRequired(false);
        
        builder.Property(t => t.UpdatedBy)
            .HasColumnType("text")
            .IsRequired(false);
        
        builder.HasOne(t => t.UserEntity)
            .WithMany(t => t.Templates)
            .HasForeignKey(t => t.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(t => t.AuthorId)
            .HasDatabaseName("IX_Templates_UserId");
    }
}