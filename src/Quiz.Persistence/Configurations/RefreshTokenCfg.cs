using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Configurations;

public class RefreshTokenCfg : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.ToTable("RefreshTokens");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.HasIndex(rt => rt.Token)
            .IsUnique()
            .HasDatabaseName("TokenIndex");
        
        builder.Property(rt => rt.Expires)
            .IsRequired();
        
        builder.Property(rt => rt.IsUsed)
            .IsRequired();
        
        builder.Property(rt => rt.IsRevoked)
            .IsRequired();
        
        builder.HasOne(rt => rt.UserEntity)
            .WithMany(u => u.RefreshToken)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}