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
            .HasMaxLength(500)
            .IsRequired();
        
        builder.Property(q => q.QuestionType)
            .HasConversion<string>();
        
        builder.HasOne(q => q.Template)
            .WithMany(q => q.Questions)
            .HasForeignKey(q => q.TemplateId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(q => q.Options)
            .HasColumnType("jsonb");
        
        builder.Property(q => q.CorrectAnswers)
            .HasColumnType("jsonb");
    }
}