using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Configurations;

public class CommentConfig : IEntityTypeConfiguration<CommentEntity>
{
    public void Configure(EntityTypeBuilder<CommentEntity> builder)
    {
        builder.ToTable("Comments");
        
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Content)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(c => c.CreatedAt);
        
        builder.Property(c => c.UpdatedAt);

        builder.Property(c => c.CreatedBy)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(c => c.UpdatedBy);

        builder.HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(c => c.Template)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.UserId)
            .HasDatabaseName("IX_UserId_Comment");
        
        builder.HasIndex(c => c.TemplateId)
            .HasDatabaseName("IX_TemplateId_Comment");
    }
}