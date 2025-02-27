using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Quiz.Api.Middlewares;
using Quiz.Api.Services;
using Quiz.Application.Abstractions;
using Quiz.Application.Common;
using Quiz.Application.Services;
using Quiz.Application.Users.Dtos;
using Quiz.Core.Abstractions;
using Quiz.Core.Repositories;
using Quiz.Persistence.Common;
using Quiz.Persistence.Repositories;

namespace Quiz.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        return services
            .AddSingleton<IGuidFactory, GuidFactory>()
            .AddScoped<IJwtManager, JwtManager>()
            .AddScoped<IAuthRepository, AuthRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ILikeRepository, LikeRepository>()
            .AddScoped<IAdminRepository, AdminRepository>()
            .AddScoped<ICommentRepository, CommentRepository>()
            .AddScoped<ITemplateRepository, TemplateRepository>()
            .AddScoped<ISubmissionRepository, SubmissionRepository>()
            .AddScoped<IRefreshTokenCookieManager, RefreshTokenCookieManager>()
            .AddScoped<IClaimsTransformation, UserClaimsTransformation>()
            .AddScoped<IDomainEventDispatcher, DomainEventDispatcher>()
            .AddTransient<UserBlockMiddleware>();
    }
    
    public static IServiceCollection AddMediatrAndFluentValidation(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                
                return new BadRequestObjectResult(
                    new ErrorResponse(ApplicationErrors.Common.ErrorResponse, errors));
            };
        });
        
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IAssemblyReference).Assembly))
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<IAssemblyReference>();
        
        return services;
    }
    
    public static IServiceCollection AddJwtSupport(this IServiceCollection services)
    {
        var jwtOptions = services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>().Value;
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.JWT_ISSUER,
                    ValidAudience = jwtOptions.JWT_AUDIENCE,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.JWT_SECRET!)),
                };
            });
        
        services.AddAuthorization();
        
        return services;
    }
    
    public static IServiceCollection AddCorsSupport(this IServiceCollection services)
    {
        return services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("http://localhost:4200", "https://vision360.kz")
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }
    
    public static void UseNginxForwardedHeaders(this IApplicationBuilder app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
    }
    
    public static void AddAuthenticationAndAuthorization(this WebApplication app)
    {
        app.UseCors("AllowFrontend");
        app.UseAuthentication();
        app.UseMiddleware<UserBlockMiddleware>();
        app.UseAuthorization();
    }
}