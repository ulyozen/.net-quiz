using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quiz.Persistence.Common;
using Quiz.Persistence.Context;
using Quiz.Persistence.Entities;

namespace Quiz.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddIdentityService(this IServiceCollection services)
    {
        services.AddIdentity<UserEntity, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }
    
    public static IServiceCollection AddEnvironmentVariables(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<DatabaseConnectionOptions>(cfg =>
        {
            cfg.User = GetEnvironmentVariable(configuration, "POSTGRES_USER");
            cfg.Password = GetEnvironmentVariable(configuration, "POSTGRES_PASSWORD");
            cfg.Host = GetEnvironmentVariable(configuration, "POSTGRES_HOST");
            if (!int.TryParse(configuration["POSTGRES_PORT"], out var port))
            {
                throw new ArgumentNullException($"Invalid {nameof(cfg.Port)} environment variable. Must be a valid integer.");
            }
            cfg.Port = port;
            cfg.Database = GetEnvironmentVariable(configuration, "POSTGRES_DB");
        });
        
        services.Configure<JwtOptions>(cfg =>
        {
            cfg.Secret = GetEnvironmentVariable(configuration, "JWT_SECRET");
            cfg.Issuer = GetEnvironmentVariable(configuration, "JWT_ISSUER");
            cfg.Audience = GetEnvironmentVariable(configuration, "JWT_AUDIENCE");
            cfg.AccessTokenExpiryMinutes = GetEnvironmentVariable(configuration, "ACCESS_TOKEN_EXPIRY_MINUTES");
            cfg.RefreshTokenExpiryDays = GetEnvironmentVariable(configuration, "REFRESH_TOKEN_EXPIRY_DAYS");
        });
        
        return services;
    }
    
    public static IServiceCollection AddDbSettings(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>((provider, options) =>
        {
            var env = provider.GetRequiredService<IOptions<DatabaseConnectionOptions>>().Value;
            options.UseNpgsql(BuildConnectionString(env));
        });
        
        return services;
    }
    
    private static string GetEnvironmentVariable(IConfiguration configuration, string key)
    {
        return configuration[key] ?? throw new ArgumentNullException($"The environment variable {key} is missing.");
    }
    
    private static string BuildConnectionString(DatabaseConnectionOptions env)
    {
        return $"User ID={env.User};Password={env.Password};Host={env.Host};Port={env.Port};Database={env.Database};";
    }
}