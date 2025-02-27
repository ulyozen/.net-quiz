using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Quiz.Api.Configuration;

public static class SwaggerConfiguration
{
    private static OpenApiSecurityScheme Scheme => new()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };
    
    public static void Configure(SwaggerGenOptions option)
    {
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
        
        option.ResolveConflictingActions(apiDesc => apiDesc.First());
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Quiz API", Version = "v1" });
        option.AddSecurityDefinition(Scheme.Reference.Id, Scheme);
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { Scheme, Array.Empty<string>() }
        });
        option.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }
}