using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Configurations;

public class QuestionConfig : IEntityTypeConfiguration<QuestionEntity>
{
    public void Configure(EntityTypeBuilder<QuestionEntity> builder)
    {
        builder.ToTable("Questions");
        
        builder.HasKey(q => q.Id);
        
        builder.Property(q => q.Title)
            .HasMaxLength(255)
            .IsRequired();
        
        builder.HasOne(q => q.QuestionType)
            .WithMany(qt => qt.Questions)
            .HasForeignKey(q => q.QuestionTypeId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(q => q.Template)
            .WithMany(t => t.Questions)
            .HasForeignKey(q => q.TemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(q => q.QuestionTypeId)
            .HasDatabaseName("IX_QuestionTypeId");
        
        builder.HasIndex(q => q.TemplateId)
            .HasDatabaseName("IX_TemplateId");
    }
}