using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Configurations;

public class TemplateTagConfig : IEntityTypeConfiguration<TemplateTag>
{
    public void Configure(EntityTypeBuilder<TemplateTag> builder)
    {
        builder.ToTable("TemplateTags");
        
        builder.HasKey(tt => new { tt.TemplateId, tt.TagId })
            .HasName("PK_Composite_TemplateTags");
        
        builder.HasOne(tt => tt.Template)
            .WithMany(t => t.TemplateTags)
            .HasForeignKey(tt => tt.TemplateId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(tt => tt.Tag)
            .WithMany(t => t.Templates)
            .HasForeignKey(tt => tt.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}