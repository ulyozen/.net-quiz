using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Quiz.Persistence.Configurations;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Context;

public class AppDbContext : IdentityDbContext<UserEntity>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new UserCfg());

        builder.ApplyConfigurationsFromAssembly(typeof(AppContext).Assembly);
    }
}