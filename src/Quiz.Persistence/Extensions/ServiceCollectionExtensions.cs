using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quiz.Persistence.Common;
using Quiz.Persistence.Context;
using Quiz.Persistence.Entities;
using Quiz.Persistence.Repositories;

namespace Quiz.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityCore(this IServiceCollection services)
    {
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
        
        return services;
    }
    
    public static IServiceCollection AddPostgreSql(this IServiceCollection services)
    {
        return services.AddDbContext<AppDbContext>((provider, options) =>
        {
            var env = provider.GetRequiredService<IOptions<PostgreSqlOptions>>().Value;
            options.UseNpgsql(BuildConnectionString(env));
        });
    }
    
    private static string BuildConnectionString(PostgreSqlOptions env)
    {
        return $"User ID={env.POSTGRES_USER};Password={env.POSTGRES_PASSWORD};Host={env.POSTGRES_HOST};Port={env.POSTGRES_PORT};Database={env.POSTGRES_DB};";
    }
}