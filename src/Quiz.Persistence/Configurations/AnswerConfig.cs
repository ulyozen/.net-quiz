using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Configurations;

public class AnswerConfig : IEntityTypeConfiguration<AnswerEntity>
{
    public void Configure(EntityTypeBuilder<AnswerEntity> builder)
    {
        builder.ToTable("Answers");
        
        builder.HasKey(a => a.Id);
        
        builder.Property(a => a.AnswerText)
            .HasColumnType("text")
            .IsRequired(false);
        
        builder.Property(a => a.AnswerValue)
            .IsRequired(false);
        
        builder.Property(a => a.AnswerBoolean)
            .IsRequired(false);
        
        builder.HasOne(a => a.Question)
            .WithMany()
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(a => a.Submission)
            .WithMany(s => s.Answers)
            .HasForeignKey(a => a.SubmissionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(c => c.QuestionId)
            .HasDatabaseName("IX_QuestionId");
        
        builder.HasIndex(c => c.SubmissionId)
            .HasDatabaseName("IX_SubmissionId");
    }
}