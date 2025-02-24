using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Persistence.Entities;

// namespace Quiz.Persistence.Configurations;

// public class QuestionTypeConfig : IEntityTypeConfiguration<QuestionTypeEntity>
// {
//     public void Configure(EntityTypeBuilder<QuestionTypeEntity> builder)
//     {
//         builder.ToTable("QuestionTypes");
//         
//         builder.HasKey(qt => qt.Id);
//         
//         builder.Property(qt => qt.ValueType)
//             .IsRequired();
//     }
// }