using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Configurations;

public class TopicConfig : IEntityTypeConfiguration<TopicEntity>
{
    public void Configure(EntityTypeBuilder<TopicEntity> builder)
    {
        builder.ToTable("Topics");
        
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .HasMaxLength(255)
            .IsRequired();
    }
}