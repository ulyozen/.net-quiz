using DotNetEnv;
using Quiz.Api.Extensions;
using Quiz.Persistence.Common;

var builder = WebApplication.CreateBuilder(args);
Env.Load();

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddEnvironmentVariables(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerConfiguration.Configure);
builder.Services.AddSignalR();

builder.Services
    .AddApplicationExtension()
    .AddJwtSupport()
    .AddDbSettings()
    .AddCorsSupport()
    .AddMediatrAndFluentValidation()
    .AddHttpContextAccessor()
    .AddRedis()
    .AddIdentityCore();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.AddAuthenticationAndAuthorization();

app.MapControllers();

app.Run();