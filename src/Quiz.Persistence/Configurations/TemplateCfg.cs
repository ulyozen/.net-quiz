using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Core.Entities;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Configurations;

public class TemplateCfg : IEntityTypeConfiguration<TemplateEntity>
{
    public void Configure(EntityTypeBuilder<TemplateEntity> builder)
    {
        throw new NotImplementedException();
    }
}