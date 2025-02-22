using DotNetEnv;
using Quiz.Api.Configuration;
using Quiz.Api.Extensions;
using Quiz.Elasticsearch.Extensions;
using Quiz.Persistence.Extensions;
using Quiz.Redis.Extensions;
using Quiz.Serilog.Extensions;
using Serilog;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerConfiguration.Configure);
builder.Services.AddSignalR();

builder.Services
    .AddEnvironmentVariables(builder.Configuration)
    .AddMediatrAndFluentValidation()
    .AddApplicationDependencies()
    .AddHttpContextAccessor()
    .AddElasticsearch()
    .AddIdentityCore()
    .AddCorsSupport()
    .AddJwtSupport()
    .AddPostgreSql()
    .AddRedis();

builder.Host.AddSerilogSupport();

var app = builder.Build();

app.UseNginxForwardedHeaders();

app.UseSerilogRequestLogging();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.AddAuthenticationAndAuthorization();

app.MapControllers();

app.Run();