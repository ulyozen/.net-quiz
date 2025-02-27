using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Configurations;

public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.ToTable("RefreshTokens");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(rt => rt.Expires)
            .IsRequired();
        
        builder.Property(rt => rt.IsUsed)
            .IsRequired();
        
        builder.Property(rt => rt.IsRevoked)
            .IsRequired();
        
        builder.HasOne(rt => rt.UserEntity)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(rt => rt.Token)
            .IsUnique()
            .HasDatabaseName("IX_RefreshTokens");
    }
}