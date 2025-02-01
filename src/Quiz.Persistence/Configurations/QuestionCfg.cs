using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Configurations;

public class QuestionCfg : IEntityTypeConfiguration<QuestionEntity>
{
    public void Configure(EntityTypeBuilder<QuestionEntity> builder)
    {
        throw new NotImplementedException();
    }
}