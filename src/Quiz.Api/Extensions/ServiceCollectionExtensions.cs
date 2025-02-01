using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Quiz.Api.Middlewares;
using Quiz.Application.Abstractions;
using Quiz.Application.Common;
using Quiz.Core.Abstractions;
using Quiz.Persistence.Common;
using Quiz.Persistence.Repositories;

namespace Quiz.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationExtension(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IAssemblyReference).Assembly));
        services.AddValidatorsFromAssemblyContaining<IAssemblyReference>();
        
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtGenerator, JwtGenerator>();
        
        // services.AddTransient<BlockedUserMiddleware>();
        
        return services;
    }

    public static IServiceCollection AddJwtSupport(this IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        
        var jwtOptions = provider.GetRequiredService<IOptions<JwtOptions>>().Value;
        
        var key = Encoding.UTF8.GetBytes(jwtOptions.Secret!);
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        
        services.AddAuthorization();
        
        return services;
    }
    
    public static void AddAuthenticationAndAuthorization(this WebApplication app)
    {
        app.UseAuthentication();
        // app.UseMiddleware<BlockedUserMiddleware>();
        app.UseAuthorization();
    }
}