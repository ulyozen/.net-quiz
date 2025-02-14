using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quiz.Application.Abstractions;
using Quiz.Application.Common;
using Quiz.Persistence.Context;
using Quiz.Persistence.Entities;
using Quiz.Persistence.Repositories;
using Quiz.Persistence.Services;
using StackExchange.Redis;

namespace Quiz.Persistence.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRedis(this IServiceCollection services)
    {
        services.AddScoped<IRedisRepository, RedisRepository>();
        services.AddScoped<IUserManager, UserManager>();
        
        services.AddSingleton<IConnectionMultiplexer>(options =>
        {
            var redisOptions = options.GetRequiredService<IOptions<RedisOptions>>().Value;
            var redisConnectionString = $"{redisOptions.Host}:{redisOptions.Port},password={redisOptions.Password}";

            return ConnectionMultiplexer.Connect(redisConnectionString);
        });
        
        services.AddSingleton<IDatabase>(options => 
            options.GetRequiredService<IConnectionMultiplexer>().GetDatabase());
        
        return services;
    }
    
    public static void AddIdentityCore(this IServiceCollection services)
    {
        
        services.AddScoped<IClaimsTransformation, UserClaimsTransformation>();

        services.AddIdentityCore<UserEntity>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddSignInManager<SignInManager<UserEntity>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }
    
    public static void AddEnvironmentVariables(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<DatabaseConnectionOptions>(cfg =>
        {
            cfg.User = GetEnvironmentVariable(configuration, "POSTGRES_USER");
            cfg.Password = GetEnvironmentVariable(configuration, "POSTGRES_PASSWORD");
            cfg.Host = GetEnvironmentVariable(configuration, "POSTGRES_HOST");
            if (!int.TryParse(configuration["POSTGRES_PORT"], out var port))
                ArgumentNullException.ThrowIfNull($"Invalid {nameof(cfg.Port)} environment variable. Must be a valid integer.");
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

        services.Configure<RedisOptions>(cfg =>
        {
            cfg.Host = GetEnvironmentVariable(configuration, "REDIS_HOST");
            cfg.Port = GetEnvironmentVariable(configuration, "REDIS_PORT");
            cfg.Password = GetEnvironmentVariable(configuration, "REDIS_PASSWORD");
        });
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