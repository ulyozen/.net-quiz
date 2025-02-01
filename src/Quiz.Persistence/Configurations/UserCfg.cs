using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Core.Entities;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Configurations;

public class UserCfg : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasIndex(u => u.NormalizedUserName)
            .IsUnique(false)
            .HasDatabaseName("UserNameIndex");
        
        builder.HasIndex(u => u.NormalizedEmail)
            .IsUnique()
            .HasDatabaseName("EmailIndex");
    }
}